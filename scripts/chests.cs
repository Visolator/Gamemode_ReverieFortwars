//ReverieFortWars/chests.cs

function RFW_SerializePosition(%pos)
{
	%x = mFloatLength(getWord(%pos, 0) * 2, 0);
	%y = mFloatLength(getWord(%pos, 1) * 2, 0);
	%z = mFloatLength(getWord(%pos, 2) * 3, 0);
	return "x" @ %x @ "y" @ %y @ "z" @ %z;
}

//undo some things
deactivatePackage(TreasureChestPersistencePackage);
deactivatePackage(TreasureChestPackage);

function serverCmdTreasureStatus() { }

//set volumes
brickTreasureChestData.volume = 256;
brickTreasureChestOpenData.volume = 256;

//these functions are already called when the brick is placed
function registerTreasureChest(%obj)
{
	%pos = RFW_SerializePosition(%obj.getPosition());
	if(!strLen($RFW::ChestSlots[%pos]))
	{
		%mat = $RFW::ResourceByColor[%obj.getColorID()];
		$RFW::ChestSlots[%pos] = %slots = mCeil($RFW::ResourceQuality[%mat] / 5);
	}
}

//TODO: ensure this is called on deletion
function unregisterTreasureChest(%obj)
{
	$RFW::ChestSlots[%pos] = "";
	return;

	%realpos = %obj.getPosition();
	%pos = RFW_SerializePosition(%realpos);
	%slots = $RFW::ChestSlots[%pos];
	for(%i = 0; %i < %slots; %i++)
	{
		%itm = $RFW::ChestItem[%pos, %i];
		$RFW::ChestItem[%pos, %i] = "";
		%type = getItemType(%itm);
		if(!%type $= "")
		{
			continue;
		}
		%data = $RFW::ItemDatablock[%type];
		%data = isObject(%data) ? %data : genericItem;
		%item = new Item()
		{
			datablock = %data;
			position = %realpos;
			data = %itm;
		};
		%name = getItemType(%itm);
		if($RFW::ItemStackable[%name])
		{
			%name = %name SPC "x" @ getItemQuantity(%itm);
		}
		%item.setShapeNameDistance(32);
		%item.setShapeName(%name);
		%item.setVelocity(getRandom(-10, 10) SPC getRandom(-10, 10) SPC getRandom(5, 10));
		//TODO: ensure fade and pop are working properly
		%item.schedulePop();
		%item.startFade(1000, 4000, true);
	}
	$RFW::ChestSlots[%pos] = "";
}