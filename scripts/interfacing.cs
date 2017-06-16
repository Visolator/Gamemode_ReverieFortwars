//ReverieFortWars/interfacing.cs

//TABLE OF CONTENTS
// #0. helper functions
// #2. BottomPrint HUD
// #3. Inventory interfacing commands
//   #3.1 /checkInventory
//   #3.2 /toPack
//   #3.3 /fromPack
//   #3.4
//   #3.5 /make
//   #3.6 /equip
//   #3.7 /unequip
// #4. Inventory BrickMenu
// #5. misc stuff
//   #5.1 centerPrint queue
//   #5.2 dropTool package

// #0.
$Hex = "0123456789ABCDEF";

function getChar(%str, %char)
{
	return getSubStr(%str, %char, 1);
}

function Dec2Hex(%v, %max)
{
	%wc = getWordCount(%v);
	if(%max < %wc)
	{
		%max = %wc;
	}
	for(%i = 0; %i < %wc; %i++)
	{
		%n = getWord(%v, %i) * 255;
		%output = %output @ getChar($Hex, mFloor(%n / 16)) @ getChar($Hex, %n % 16);
	}
	return %output;
}

function Hex2Dec(%str)
{
	if(getChar(%str, 0) $= "#")
	{
		%str = getSubStr(%str, 1, strLen(%str));
	}
	%r = (striPos($Hex, getChar(%str, 0)) * 16 + striPos($Hex, getChar(%str, 1))) / 255;
	%g = (striPos($Hex, getChar(%str, 2)) * 16 + striPos($Hex, getChar(%str, 3))) / 255;
	%b = (striPos($Hex, getChar(%str, 4)) * 16 + striPos($Hex, getChar(%str, 5))) / 255;
	return %r SPC %g SPC %b;
}

function max(%a, %b)
{
	return %a > %b ? %a : %b;
}

function colorNormalize(%col)
{
	%max = 0;
	for(%i = 0; %i < 3; %i++)
	{
		%max = max(%max, getWord(%col, %i));
	}
	return vectorScale(%col, %max == 0 ? 1 : 1 / %max);
}


// #2.
$BrickImage = BrickImage.getID();

function Player::UpdateBottomPrintHUD(%obj, %mode, %data)
{
	if(!isObject(%client = %obj.client))
	{
		return;
	}
	%time = getSimTime();
	switch$(%mode)
	{	
		// case "Combat": //%data is the name of the opponent TAB its percent HP e.g. "Ghost\t90"
			// %op = getField(%data, 0);
			// %hp = getField(%data, 1);
			// %dataStr = "<color:FF0000>" @ %op @ " \c6(" @ mFloatLength(%hp, 0) @ "%)" @ "     ";
			// %obj.lastMode = %mode;
			// %obj.lastDataTime = %time;
		case "Digging": //%data is the type of brick TAB its current HP TAB its max HP e.g. "Sand\t50\t100"
			%mat = getField(%data, 0);
			%hp = getField(%data, 1);
			%maxHp = getField(%data, 2);
			%dataStr = "\c6" @ %mat @ " block (" @ %hp @ "/" @ %maxHp @ ")" @ "     ";
			%obj.lastMode = %mode;
			%obj.lastDataTime = %time;
		case "Building": //%data is the current relevant resource (by paint color) TAB the amount of that resource the player has e.g. "0\t512"
			%n = getField(%data, 0);
			%quantity = getField(%data, 1);
			%dataStr = "\c6" @ $RFW::ResourceByColor[%n] @ ": <color:" @ Dec2Hex(getColorIdTable(%n), 3) @ ">" @ %quantity;
			if(isObject(%brick = %obj.tempBrick))
			{
				%dataStr = %dataStr @ "/" @ %brick.getVolume();
			}
			else if(%obj.getMountedImage(0) == $BrickImage)
			{
				%dataStr = %dataStr @ "/" @ %client.instantUseData.getVolume();
			}
			%dataStr = %dataStr @ "     ";
			%obj.lastMode = %mode;
			%obj.lastDataTime = %time;
		// case "Examine":
			// %obj.lastMode = %mode;
			// %obj.lastDataTime = %time;
		default:
			%dataStr = %obj.lastData;
	}
	if(%obj.lastDataTime + 15000 < %time)
	{
		%obj.lastDataTime = %time;
		%dataStr = "";
	}
	%obj.lastData = %dataStr;
	//territory
	%pos = %obj.getPosition();
	%closestTotem = 0;
	%closestTotemDist = $RFW::TotemRadius + 1;
	%groupCount = mainBrickGroup.getCount();
	for(%i = 1; %i < %groupCount; %i++)
	{
		%totem = mainBrickGroup.getObject(%i).totem;
		if(isObject(%totem))
		{
			%dist = vectorDist(%totem.getPosition(), %pos);
			if(%dist < %closestTotemDist)
			{
				%closestTotem = %totem;
				%closestTotemDist = %dist;
			}
		}
	}
	if(isObject(%closestTotem))
	{
		%group = %closestTotem.getGroup();
		%color =  %group.tribe $= %client.account.read("Tribe") ? "\c2" : "<color:FF0000>";
		%dataStr = %dataStr @ %color @ %group.tribe @ " territory";
	}
	//position
	%x = mFloatLength(getWord(%pos, 0) / 8, 0);
	%y = mFloatLength(getWord(%pos, 1) / 8, 0);
	%z = mFloatLength(getWord(%pos, 2) / 8, 0);
	%pos = "<just:right>\c6Position: \c3" @ %x SPC %y SPC %z @ " ";
	//hp
	%dmgPercent = %obj.getDamagePercent();
	%hpColor = colorNormalize(vectorAdd("0 1 0", vectorScale("1 -1 0", %dmgPercent)));
	%hpColor = "<color:" @ Dec2Hex(%hpColor, 3) @ ">";
	%line2 = "\n<just:left>\c6Health: " @ %hpColor @ mFloatLength((1 - %dmgPercent) * 100, 0) @ "%";
	//direction
	%vec = %obj.getForwardVector();
	%x = getWord(%vec, 0);
	%y = getWord(%vec, 1);
	%vec = "<just:right>\c6Compass: \c3";
	if(mAbs(%x) > mAbs(%y))
	{
		%vec = %vec @ (%x > 0 ? "East " : "West ");
	}
	else
	{
		%vec = %vec @ (%y > 0 ? "North " : "South ");
	}
	%client.bottomPrint(%dataStr @ %pos @ %line2 @ %vec, 6, true);
	cancel(%obj.hudUpdateSched);
	%obj.hudUpdateSched = %obj.schedule(200, UpdateBottomPrintHUD);
}

package RFW_Interfacing
{
	function GameConnection::spawnPlayer(%client)
	{
		%p = parent::spawnPlayer(%client);
		if(isObject(%obj = %client.player)) //probably not necessary
		{
			%obj.clearTools();
			%acc = %client.account;
			%maxEquips = %acc.read("MaxEquips");
			for(%i = 0; %i < %maxEquips; %i++)
			{
				%eq = %acc.read("Equip" @ %i);
				%eq = $RFW::ItemDatablock[getItemType(%eq)];
				%obj.tool[%i] = %eq;
				messageClient(%client, 'MsgItemPickup', "", %i, %eq, 1);
			}
			%obj.UpdateBottomPrintHUD();
		}
		return %p;
	}

	function GameConnection::getSpawnPoint(%client)
	{
		%p = parent::getSpawnPoint(%client);
		if(getWord(%p, 2) > 1)
		{
			return %p;
		}
		while(%i++ < 20)
		{
			%x = getRandom(683, 1365);
			%y = getRandom(683, 1365);
			%ray = containerRaycast(%x SPC %y SPC 700, %x SPC %y SPC 0, $Typemasks::FxBrickObjectType);
			if(isObject(%hit = firstWord(%ray)))
			{
				%pos = %hit.getPosition();
				%pos = vectorAdd(%pos, "0 0 " @ (0.1 * %hit.getDatablock().brickSizeZ));
				return %pos;
			}
		}
		error("crashcheck for pickSpawnPoint failure");
		return %p;
	}

	function serverCmdLight(%client)
	{
		%p = parent::serverCmdLight(%client);
		if(isObject(%light = %client.player.light))
		{
			%light.setDatablock(OrangeDimAmbientLight);
		}
		return %p;
	}

	function serverCmdDropTool(%client, %tool)
	{
		%obj = %client.player;
		if(!isObject(%obj))
		{
			return;
		}
		%acc = %client.account;
		if(%client.BCM_MenuState == 2)
		{
			%itemData = stripMLcontrolChars(getRecord(%client.BCM_MenuCache, %client.BCM_MenuPosition));
			if(%itemData $= "Fists")
			{
				messageClient('MsgError');
				%client.centerPrintQueue("<color:FF0000>You'll have to cut them off first.", 3);
				return;
			}
			if($RFW::ItemStackable[%itemData])
			{
				%q = %client.BCM_MenuQuantity;
				if(%q == 0)
				{
					return;
				}
				%name = %itemData @ " x" @ %q;
				%itemData = %itemData TAB %q;
			}
			else
			{
				%name = %itemData;
			}
			if(!%client.account.removeItem(%itemData, false))
			{
				return;
			}
			%temp = %obj.tool[%tool];
			%datablock = $RFW::ItemDatablock[getField(%itemData, 0)];
			%obj.tool[%tool] = %datablock ? %datablock : nameToID(genericItem);
			%p = parent::serverCmdDropTool(%client, %tool);
			%obj.tool[%tool] = %temp;
			messageClient(%client, 'MsgItemPickup', "", %tool, %temp, 1);
			InitContainerRadiusSearch(%obj.getPosition(), 5, $TypeMasks::ItemObjectType);
			while(%hit = containerSearchNext())
			{
				if(!%hit.RFW_Searched)
				{
					%hit.RFW_Searched = true;
					%hit.setShapeNameDistance(32);
					%hit.setShapeName(%name);
					%hit.data = %itemData;
					%found = true;
					break;
				}
			}
			%client.BCM_MenuChangeState(2);
		}
		else
		{
			if(%obj.tool[%tool] == nameToID(FistsItem))
			{
				messageClient('MsgError');
				%client.centerPrintQueue("<color:FF0000>You'll have to cut them off first.", 3);
				return;
			}
			%p = parent::serverCmdDropTool(%client, %tool);
			%itemData = %acc.read("Equip" @ %tool);
			%name = getItemType(%itemData);
			if($RFW::ItemStackable[%name])
			{
				%name = %name SPC "x" @ getItemQuantity(%itemData);
			}
			InitContainerRadiusSearch(%obj.getPosition(), 5, $TypeMasks::ItemObjectType);
			while(%hit = containerSearchNext())
			{
				if(!%hit.RFW_Searched)
				{
					%hit.RFW_Searched = true;
					%hit.setShapeNameDistance(32);
					%hit.setShapeName(%name);
					%hit.data = %itemData;
					%found = true;
					break;
				}
			}
			if(%found)
			{
				%acc.set("Equip" @ %tool, "");
			}
		}
		return %p;
	}

	function armor::onCollision(%this, %obj, %col, %norm, %speed)
	{
		if(%col.getClassName() $= "Item")
		{
			if(%obj.getClassName() $= "AIPlayer")
			{
				//bots shouldn't pick up items
				return;
			}
			else if(isObject(%acc = (%client = %obj.client).account) && strLen(%data = %col.data))
			{
				if(!%acc.addItem(%data, true))
				{
					messageClient("<color:FF0000>No pack space!", 3);
					return;
				}
				else
				{
					%col.delete();
					return;
				}
			}
		}
		return parent::onCollision(%this, %obj, %col, %norm, %speed);
	}
};
activatePackage(RFW_Interfacing);

// #3.

// #3.1
function serverCmdCheckInventory(%client, %w1, %w2, %w3, %w4, %w5)
{
	%w = trim(%w1 SPC %w2 SPC %w3 SPC %w4 SPC %w5);
	%w = $RFW::Item[$RFW::Item[%w]]; //this will force its capitalization to be correct
	%acc = %client.account;
	%maxItems = %acc.read("MaxItems");
	if($RFW::Item[%w] $= "")
	{
		//all the items
		for(%i = 0; %i < %maxItems; %i++)
		{
			%item = %acc.read("Item" @ %i);
			if(%item !$= "")
			{
				%type = getItemType(%item);
				%str = "\c3(#" @ %i @ ") \c2";
				if($RFW::ItemStackable[%type])
				{
					%str = %str @ getItemQuantity(%item) @ "x " @ %type;
				}
				else
				{
					%str = %str @ %type @ " (" @ (getItemKills(%item) * 1) @ " kills)";
				}
				messageClient('', %str);
			}
		}
		return;
	}
	for(%i = 0; %i < %maxItems; %i++)
	{
		%item = %acc.read("Item" @ %i);
		if(getItemType(%item) $= %w)
		{
			if($RFW::ItemStackable[%w])
			{
				%quantity = getItemQuantity(%item);
				break;
			}
			else
			{
				%quantity++;
			}
		}
	}
	if(%quantity > 0)
	{
		%client.centerPrintQueue("\c2You have " @ %quantity @ "x " @ %w @ ".", 3);
	}
	else
	{
		messageClient('MsgError');
		%client.centerPrintQueue("<color:FF0000>You don't have any " @ %w @ ".", 3);
	}
}

// #3.2
function serverCmdToPack(%client, %w1, %w2, %w3, %w4, %w5)
{
	%w = trim(%w1 SPC %w2 SPC %w3 SPC %w4 SPC %w5);
	if(!strLen(%w))
	{
		//try to move the currently equipped item
		if(isObject(%obj = %client.player))
		{
			%w = %obj.getMountedImage(0).item.uiName;
		}
	}
	else
	{
		%w = $RFW::Item[$RFW::Item[%w]]; //this will force its capitalization to be correct
	}
	if($RFW::Item[%w] $= "")
	{
		messageClient('MsgError');
		%client.centerPrintQueue("<color:FF0000>There is not an item named \"" @ %w @ "\".", 3);
		return;
	}
	%acc = %client.account;
	%maxEquips = %acc.read("MaxEquips");
	for(%i = 0; %i < %maxEquips; %i++)
	{
		%item = %acc.read("Equip" @ %i);
		if(getItemType(%item) $= %w)
		{
			%found = true;
			break;
		}
	}
	if(!%found)
	{
		messageClient('MsgError');
		%client.centerPrintQueue("<color:FF0000>You don't have a " @ %w @ " equipped.", 3);
	}
	else
	{
		if(%acc.addItem(%item))
		{
			if(isObject(%obj = %client.player))
			{
				if(%obj.currTool == %i)
				{
					serverCmdUnuseTool(%client);
				}
				%obj.tool[%i] = "";
				messageClient(%client, 'MsgItemPickup', '', %i, 0);
			}
			%acc.set("Equip" @ %i, "");
			%client.centerPrintQueue("\c2Moved " @ %w @ " to pack successfully.", 3);
		}
		else
		{
			messageClient('MsgError');
			%client.centerPrintQueue("<color:FF0000>You don't have room in your pack for a " @ %w @ ".", 3);
		}
	}
}

// #3.3
function serverCmdFromPack(%client, %w1, %w2, %w3, %w4, %w5)
{
	%w = trim(%w1 SPC %w2 SPC %w3 SPC %w4 SPC %w5);
	if(!strLen(%w))
	{
		messageClient('MsgError');
		%client.centerPrintQueue("<color:FF0000>You must enter an item name.", 3);
		return;
	}
	else
	{
		%w = $RFW::Item[$RFW::Item[%w]]; //this will force its capitalization to be correct
	}
	if($RFW::Item[%w] $= "")
	{
		messageClient('MsgError');
		%client.centerPrintQueue("<color:FF0000>There is not an item named \"" @ %w @ "\".", 3);
		return;
	}
	else if(!(%itemDB = $RFW::ItemDatablock[%w]))
	{
		messageClient('MsgError');
		%client.centerPrintQueue("<color:FF0000>" @ %w @ " cannot be equipped.", 3);
		return;
	}
	%acc = %client.account;
	%i = %acc.getItemSlot(%w);
	if(%i == -1)
	{
		messageClient('MsgError');
		%client.centerPrintQueue("<color:FF0000>You don't have a " @ %w @ ".", 3);
		return;
	}
	%item = %acc.read("Item" @ %i);
	%maxEquips = %acc.read("MaxEquips");
	for(%j = 0; %j < %maxEquips; %j++)
	{
		if(%acc.read("Equip" @ %j) $= "")
		{
			%acc.set("Item" @ %i, "");
			%acc.set("Equip" @ %j, %item);
			%found = true;
			break;
		}
	}
	if(!%found)
	{
		messageClient('MsgError');
		%client.centerPrintQueue("<color:FF0000>No room in your equipment for a " @ %w @ ".", 3);
		return;
	}
	if(isObject(%obj = %client.player))
	{
		%obj.tool[%j] = %itemDB;
		messageClient(%client, 'MsgItemPickup', '', %j, %itemDB);
	}
	%client.centerPrintQueue("\c2" @ %w @ " added to equipment.", 3);
}

//3.4
function serverCmdNameWeapon(%client, %w1, %w2, %w3, %w4, %w5)
{
	%w = trim(%w1 SPC %w2 SPC %w3 SPC %w4 SPC %w5);
	messageClient(%client, 'MsgError', "not yet implemented");
}

// #3.5
function serverCmdCraft(%client, %w1, %w2, %w3, %w4, %w5)
{
	%w = trim(%w1 SPC %w2 SPC %w3 SPC %w4 SPC %w5);
	%w1 = firstWord(%w);
	if(%w1 $= %w1 * 1)
	{
		%q = mFloatLength(%w1, 0);
		if(%q < 1)
		{
			messageClient(%client, 'MsgError');
			%client.centerPrintQueue("<color:FF0000>Cannot craft " @ %q @ " of an item.", 3);
			return;
		}
		%w = restWords(%w);
	}
	else
	{
		%q = 1;
	}
	%it = $RFW::Item[$RFW::Item[%w]];
	if(%it $= "")
	{
		messageClient(%client, 'MsgError');
		%client.centerPrintQueue("<color:FF0000>There is not an item named \"" @ %w @ "\".", 3);
		return;
	}
	%matsLists = $RFW::RecipeMats[%it];
	%reqs = $RFW::RecipeReqs[%it];
	if(%matsLists $= "")
	{
		messageClient(%client, 'MsgError');
		%client.centerPrintQueue("<color:FF0000>There is not a recipe to craft \"" @ %it @ "\".", 3);
		return;
	}
	%acc = %client.account;
	%rcount = getRecordCount(%matsLists);
	//check materials first
	for(%i = 0; %i < %rcount; %i++)
	{
		%tfailed = false;
		%matsList = getRecord(%matsLists, %i);
		%tcount = getFieldCount(%matsList);
		for(%j = 0; %j < %tcount; %j++)
		{
			%mat = getField(%matsList, %j);
			%mat = restWords(%mat) TAB (%q * firstWord(%mat));
			if(!%acc.hasItemQuantity(%mat) && !%client.canCraftAnything)
			{
				%tfailed = true;
				messageClient(%client, 'MsgError');
				%q = getField(%mat, 1);
				%client.centerPrintQueue("\c0Requires " @ getField(%mat, 1) @ "x " @ getField(%mat, 0) @ " (You have: " @ %acc.getItemQuantity(%mat) @ "x)", 3);
				break;
			}

			if(%requireStr $= "")
				%requireStr = getField(%mat, 0) @ "x" @ getField(%mat, 1);
			else
				%requireStr = %requireStr NL getField(%mat, 0) @ "x" @ getField(%mat, 1);
		}
		if(!%tfailed)
		{
			%success = true;
			break;
		}
	}
	if(!%success)
	{
		return;
	}
	%success = false;

	//check requirements
	%tcount = getFieldCount(%reqs);
	for(%i = 0; %i < %tcount; %i++)
	{
		if(%acc.hasItem(getField(%reqs, %i)))
		{
			%success = true;
			break;
		}
	}

	if(!%success && %tcount > 0 && !%client.canCraftAnything)
	{
		messageClient(%client, 'MsgError');
		%client.centerPrintQueue("<color:FF0000>Requires " @ getField(%reqs, 0) @ " or better.", 3);
		return;
	}

	%client.matQ = %q;
	%client.matL = %matsList;
	%client.matIT = %it;

	%client.craftConfirm = 1;
	%client.lastCraftAttempt = getSimTime();
	commandToClient(%client, 'MessageBoxOKCancel', "Craft " @ %w @ "?", "Requirements:\n" @ %requireStr, 'ConfirmCraft');
}

function serverCmdConfirmCraft(%client)
{
	if(getSimTime() - %client.lastCraftAttempt < 10000 && %client.craftConfirm)
	{
		if(!isObject(%acc = %client.account))
		{
			%client.chatMessage("You have an invalid account. Please talk to an administrator.");
			return;
		}

		%client.craftConfirm = 0;
		%q = %client.matQ;
		%matsList = %client.matL;
		%it = %client.matIT;

		if(%q $= "" || %matsList $= "" || %it $= "")
		{
			%client.chatMessage("You did not request an actual crafting item.");
			return;
		}

		//remove materials and add item
		if(!%client.canCraftAnything)
		{
			%tcount = getFieldCount(%matsList);
			for(%i = 0; %i < %tcount; %i++)
			{
				%mat = getField(%matsList, %i);
				%mat = restWords(%mat) TAB (%q * firstWord(%mat));
				if(!%acc.removeItem(%mat))
				{
					error("Material removal failure");
					%acc.dump();
					%client.centerPrintQueue("\c4Material removal failure", 3);
				}
			}
		}
		
		if($RFW::ItemStackable[%it])
		{
			%it = %it TAB %q TAB "";
			%acc.addItem(%it, true);
		}
		else
		{
			%it = %it TAB 0 TAB "";
			for(%i = 0; %i < %q; %i++)
			{
				if(!%acc.addItem(%it TAB 0 TAB "", true))
				{
					%client.centerPrintQueue("\c4Out of inventory space", 3);
					break;
					//TODO: handle this better
				}
			}
		}
	}
}

function serverCmdMake(%client, %w1, %w2, %w3, %w4, %w5)
{
	serverCmdCraft(%client, %w1, %w2, %w3, %w4, %w5);
}

// #3.6
function serverCmdEquip(%client, %w1, %w2, %w3, %w4, %w5)
{
	%w = trim(%w1 SPC %w2 SPC %w3 SPC %w4 SPC %w5);
	%it = $RFW::Item[$RFW::Item[%w]];
	%acc = %client.account;
	if(%it $= "")
	{
		messageClient(%client, 'MsgError');
		%client.centerPrintQueue("<color:FF0000>There is not an item named \"" @ %w @ "\".", 3);
		return;
	}
	%armor = %acc.getItem(%it);
	if(!%armor $= "0")
	{
		messageClient(%client, 'MsgError');
		%client.centerPrintQueue("<color:FF0000>You don't have " @ %it @ ".", 3);
		return;
	}
	if(!$RFW::ItemWearable[%it])
	{
		if($RFW::ItemEquipable[%it])
		{
			serverCmdFromPack(%client, %w);
			return;
		}
		if(%fn = $RFW::ItemEffect[%it])
		{
			if(isObject(%player = %client.player))
			{
				return eval(%fn);
			}
			else
			{
				messageClient(%client, 'MsgError');
				%client.centerPrintQueue("<color:FF0000>Cannot use \"" @ %it @ "\" right now.", 3);
			}
		}
		messageClient(%client, 'MsgError');
		%client.centerPrintQueue("<color:FF0000>Cannot equip \"" @ %it @ "\".", 3);
		return;
	}
	%acc.removeItem(%armor, false);
	%type = $RFW::ItemSlot[%it];
	if((%equipped = %acc.read(%type)) !$= "")
	{
		serverCmdUnequip(%client, %equipped);
	}
	%acc.set(%type, %it);
	%client.centerPrintQueue("\c2Equipped " @ %it @ ".", 3);
	if(isObject(%obj = %client.player))
	{
		%client.applyBodyParts();
		%client.applyBodyColors();
	}
}

// #3.7
function serverCmdUnequip(%client, %w1, %w2, %w3, %w4, %w5)
{
	%w = trim(%w1 SPC %w2 SPC %w3 SPC %w4 SPC %w5);
	%it = $RFW::Item[$RFW::Item[%w]];
	if(%it $= "")
	{
		messageClient(%client, 'MsgError');
		%client.centerPrintQueue("<color:FF0000>There is not an item named \"" @ %w @ "\".", 3);
		return;
	}
	if(!$RFW::ItemWearable[%it])
	{
		if($RFW::ItemEquipable[%it])
		{
			serverCmdToPack(%client, %w);
			return;
		}
		messageClient(%client, 'MsgError');
		%client.centerPrintQueue("<color:FF0000>Cannot equip \"" @ %it @ "\".", 3);
		return;
	}
	%acc = %client.account;
	%type = $RFW::ItemSlot[%it];
	%item = %acc.read(%type);
	if(%item $= "")
	{
		messageClient(%client, 'MsgError');
		%client.centerPrintQueue("<color:FF0000>Nothing equipped in that slot.", 3);
		return;
	}
	if(%acc.addItem(%item, false))
	{
		%client.centerPrintQueue("\c2Unequipped " @ getItemType(%item) @ ".", 3);
		switch$(getWord(%it, 1))
		{
			case "Armor": %acc.set("Armor", "");
			case "Shirt": %acc.set("Shirt", "");
			case "Pants": %acc.set("Pants", "");
			case "Hat": %acc.set("Hat", "");
		}
		if(isObject(%obj = %client.player))
		{
			%client.applyBodyParts();
			%client.applyBodyColors();
		}
	}
	else
	{
		messageClient(%client, 'MsgError');
		%client.centerPrintQueue("<color:FF0000>No room in your inventory for the " @ %it @ ".", 3);
	}
	return;
}

// #5.
// #5.1
function GameConnection::centerPrintQueue(%client, %text, %time)
{
	if(%client.BCM_MenuState != 0 || %client.getControlObject() != %client.player || !%client.account.read("UseCPQ"))
	{
		messageClient(%client, '', %text);
		return;
	}
	if(%time < 1000)
	{
		%time*= 1000;
	}
	RFW_CPQ_Enqueue(%client, %text, %time);
}

function RFW_CPQ_Update(%client)
{
	if(!isObject(%client) || %client.BCM_MenuState != 0 || %client.getControlObject() != %client.player)
	{
		return;
	}
	%text = getFields(%client.RFW_CPQ[0], 1, getFieldCount(%client.RFW_CPQ[0]));
	for(%i = 1; (%str = %client.RFW_CPQ[%i]) !$= ""; %i++)
	{
		%text = %text NL getFields(%str, 1, getFieldCount(%str));
	}
	%client.centerPrint(%text, 5);
}

function RFW_CPQ_Enqueue(%client, %text, %time)
{
	%text = getRandom() TAB %text;
	for(%i = 4; %i > 0; %i--)
	{
		%client.RFW_CPQ[%i] = %client.RFW_CPQ[%i - 1];
	}
	%client.RFW_CPQ[0] = %text;
	schedule(%time, 0, RFW_CPQ_Dequeue, %client, %text);
	RFW_CPQ_Update(%client);
}

function RFW_CPQ_Dequeue(%client, %text)
{
	for(%i = 0; %i < 5; %i++)
	{
		if(!%found)
		{
			if(%client.RFW_CPQ[%i] $= %text)
			{
				%found = true;
				%i--;
			}
		}
		else
		{
			%client.RFW_CPQ[%i] = %client.RFW_CPQ[%i + 1];
		}
	}
	RFW_CPQ_Update(%client);
}