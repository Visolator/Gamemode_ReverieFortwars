//ReverieFortWars/building.cs

//TABLE OF CONTENTS


function serverCmdUseSprayCan(%client, %n)
{
	%n = mClamp(%n, 0, 63);
	%client.currentFxCan = 0;
	%client.currentColor = %n;
	if(isObject(%obj = %client.player))
	{
		if(isObject(%brick = %obj.tempBrick))
		{
			%brick.setColor(%n);
		}
		%acc = %client.account;
		%res = $RFW::ResourceByColor[%n];
		%item = %acc.getItem(%res);
		%obj.updateBottomPrintHUD("Building", %n TAB mFloatLength(getItemQuantity(%item), 1));
	}
}

function serverCmdUseFxCan(%client, %n)
{
	%client.currentFxCan = %n;
}

function serverCmdUndoBrick(%client)
{
	%op = %client.undoStack.pop();
	if(getField(%op, 1) $= "PLANT")
	{
		if(isObject(%brick = getField(%op, 0)))
		{
			%brick.damage(%client, "INF");
		}
	}
	else if(!isObject(getField(%op, 0)) && %op !$= "")
	{
		schedule(0, 0, serverCmdUndoBrick, %client);
	}
}

if(isPackage("RFW_Building"))
	deactivatePackage("RFW_Building");

package RFW_Building
{
	function serverCmdUseInventory(%client, %inv)
	{
		parent::serverCmdUseInventory(%client, %inv);
		if(isObject(%obj = %client.player))
		{
			%q = getItemQuantity(%client.account.getItem($RFW::ResourceByColor[%client.currentColor]));
			%obj.updateBottomPrintHUD("Building", %client.currentColor TAB mFloatLength(%q, 1));
		}
	}

	function serverCmdShiftBrick(%client, %x, %y, %z)
	{
		parent::serverCmdShiftBrick(%client, %x, %y, %z);
		if(!isObject(%client.player.tempBrick))
		{
			return %brick;
		}
		if(isObject(%obj = %client.player))
		{
			%q = getItemQuantity(%client.account.getItem($RFW::ResourceByColor[%client.currentColor]));
			%obj.updateBottomPrintHUD("Building", %client.currentColor TAB mFloatLength(%q, 1));
		}
	}

	function fxDtsBrick::onPlant(%brick)
	{
		Parent::onPlant(%brick);
		%brick.schedule(0, "RFW_Init");
	}

	function serverCmdPlantBrick(%client)
	{
		if(!isObject(%client.player.tempBrick))
		{
			parent::serverCmdPlantBrick(%client);
			return;
		}

		if(isObject(%temp = %client.player.tempBrick) && %temp.getClassName() $= "fxDtsBrick")
		{
			%acc = %client.account;
			%mat = $RFW::ResourceByColor[%client.currentColor];
			%vol = %temp.getVolume();
			%it = %mat TAB %vol;
			if(%acc.hasItemQuantity(%it))
			{
				//find nearest totem
				%pos = %temp.getPosition();
				%closestTotem = 0;
				%closestTotemDist = $RFW::TotemRadius;
				for(%i = 0; %i < mainBrickGroup.getCount(); %i++)
				{
					%totem = mainBrickGroup.getObject(%i).totem;
					if(isObject(%totem))
					{
						%dist = vectorDist(%pos, %totem.getPosition());
						if(%dist < %closestTotemDist)
						{
							%closestTotemDist = %dist;
							%closestTotem = %totem;
						}
					}
				}

				if(isObject(%closestTotem) && getTrustLevel(%client, %closestTotem) < 1)
				{
					messageClient(%client, 'MsgError');
					%client.centerPrintQueue("<color:FF0000>Cannot build in other players' territory.", 3);
					if(isObject(%obj = %client.player))
						%obj.updateBottomPrintHUD("Building", %client.currentColor TAB mFloatLength(getItemQuantity(%acc.getItem(%mat)), 1));

					return;
				}
				else
				{
					%brick = parent::serverCmdPlantBrick(%client);
					if(!isObject(%brick))
						return %brick;

					%brick.setColorFX($RFW::ResourceColorFx[%mat]);
					%brick.mat = %mat;
				}

				if(isObject(%obj = %client.player))
					%obj.updateBottomPrintHUD("Building", %client.currentColor TAB mFloatLength(getItemQuantity(%acc.getItem(%mat)), 1));
			}
			else
			{
				messageClient(%client, 'MsgError');
				%client.centerPrintQueue("<color:FF0000>Not enough " @ %mat @ ".", 3);
				return;
			}
		}

		return %brick;
	}
};
activatePackage(RFW_Building);

function fxDtsBrick::RFW_Init(%brick)
{
	if(!isObject(%client = %brick.client))
		return;

	if(!isObject(%acc = %client.account))
		return;

	%acc.removeItem(%brick.mat TAB %brick.getVolume());
}