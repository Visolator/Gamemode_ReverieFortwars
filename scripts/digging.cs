//ReverieFortWars/digging.cs

//TABLE OF CONTENTS
// #1. parameters
// #2. sounds
// #3. functionality

// #1.

function RFW_Digging_Initialize()
{
	$RFW::WorldBasis = nameToID(brick16xCubeData);
	$RFW::WorldBasisName = $RFW::WorldBasis.uiName;

	$RFW::WorldBasisSizeX = $RFW::WorldBasis.brickSizeX * 0.5;
	$RFW::WorldBasisSizeY = $RFW::WorldBasis.brickSizeY * 0.5;
	$RFW::WorldBasisSizeZActual = $RFW::WorldBasis.brickSizeZ * 0.2;

	Brick4xCubeData.isCube = true;
	Brick8xCubeData.isCube = true;
	Brick16xCubeData.isCube = true;
}
schedule(10, 0, RFW_Digging_Initialize);

$RFW::Ores["Stone"] = 4;
$RFW::Ore["Stone", 0] = 0.6 SPC "Obsidian";
$RFW::Ore["Stone", 1] = 0.7 SPC "Surtra";
$RFW::Ore["Stone", 2] = 0.9 SPC "Calcite";
$RFW::Ore["Stone", 3] = 1.0 SPC "Sanguite";

$RFW::Ores["Sand"] = 4;
$RFW::Ore["Sand", 0] = 0.1 SPC "Stone";
$RFW::Ore["Sand", 1] = 0.5 SPC "Surtra";
$RFW::Ore["Sand", 2] = 0.9 SPC "Calcite";
$RFW::Ore["Sand", 3] = 1.0 SPC "Sanguite";

$RFW::Ores["Snow"] = 4;
$RFW::Ore["Snow", 0] = 0.1 SPC "Stone";
$RFW::Ore["Snow", 1] = 0.6 SPC "Surtra";
$RFW::Ore["Snow", 2] = 0.85 SPC "Calcite";
$RFW::Ore["Snow", 3] = 1.0 SPC "Sanguite";

$RFW::Ores["Dirt"] = 3;
$RFW::Ore["Dirt", 0] = 0.4 SPC "Stone";
$RFW::Ore["Dirt", 1] = 0.85 SPC "Surtra";
$RFW::Ore["Dirt", 2] = 1.0 SPC "Calcite";

$RFW::Ores["Flesh"] = 3;
$RFW::Ore["Flesh", 0] = 0.2 SPC "Bone";
$RFW::Ore["Flesh", 0] = 0.7 SPC "Sanguite";
$RFW::Ore["Flesh", 1] = 1.0 SPC "Calcite";

$PublicClient = new ScriptObject(){ brickGroup = Brickgroup_888888; BL_ID = 888888; };

// #2.

datablock AudioProfile(dirtHitSound1)
{
	filename = "Add-Ons/Gamemode_ReverieFortWars/sounds/dirt_hit_1.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(dirtHitSound2)
{
	filename = "Add-Ons/Gamemode_ReverieFortWars/sounds/dirt_hit_2.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(dirtHitSound3)
{
	filename = "Add-Ons/Gamemode_ReverieFortWars/sounds/dirt_hit_3.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(dirtHitSound4)
{
	filename = "Add-Ons/Gamemode_ReverieFortWars/sounds/dirt_hit_4.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(dirtHitSound5)
{
	filename = "Add-Ons/Gamemode_ReverieFortWars/sounds/dirt_hit_5.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(dirtHitSound6)
{
	filename = "Add-Ons/Gamemode_ReverieFortWars/sounds/dirt_hit_6.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(dirtHitSound7)
{
	filename = "Add-Ons/Gamemode_ReverieFortWars/sounds/dirt_hit_7.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(plantHitSound)
{
	filename = "Add-Ons/Gamemode_ReverieFortWars/sounds/plant_hit.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(sandHitSound1)
{
	filename = "Add-Ons/Gamemode_ReverieFortWars/sounds/sand_hit_1.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(sandHitSound2)
{
	filename = "Add-Ons/Gamemode_ReverieFortWars/sounds/sand_hit_2.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(sandHitSound3)
{
	filename = "Add-Ons/Gamemode_ReverieFortWars/sounds/sand_hit_3.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(stoneHitSound1)
{
	filename = "Add-Ons/Gamemode_ReverieFortWars/sounds/stone_hit_1.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(stoneHitSound2)
{
	filename = "Add-Ons/Gamemode_ReverieFortWars/sounds/stone_hit_2.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(stoneHitSound3)
{
	filename = "Add-Ons/Gamemode_ReverieFortWars/sounds/stone_hit_3.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(stoneHitSound4)
{
	filename = "Add-Ons/Gamemode_ReverieFortWars/sounds/stone_hit_4.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(stoneHitSound5)
{
	filename = "Add-Ons/Gamemode_ReverieFortWars/sounds/stone_hit_5.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(metalHitSound)
{
	filename = "Add-Ons/Gamemode_ReverieFortWars/sounds/metal_hit.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(fleshHitSound1)
{
	filename = "Add-Ons/Gamemode_ReverieFortWars/sounds/flesh_hit_1.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(fleshHitSound2)
{
	filename = "Add-Ons/Gamemode_ReverieFortWars/sounds/flesh_hit_2.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(fleshHitSound3)
{
	filename = "Add-Ons/Gamemode_ReverieFortWars/sounds/flesh_hit_3.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(glassHitSound)
{
	filename = "Add-Ons/Gamemode_ReverieFortWars/sounds/glass_hit.wav";
	description = AudioClose3d;
	preload = true;
};

// #3.
function fxDtsBrick::getVolume(%this)
{
	%data = %this.getDatablock();
	if(%vol = %data.volume)
	{
		return %vol;
	}
	else
	{
		%vol = %data.brickSizeX * %data.brickSizeY * %data.brickSizeZ * 0.05;
		%data.volume = %vol;
		return %vol;
	}
}

function RFW_GetOffset(%z)
{
	//modulus is a better way to do this but torque's modulus function isn't cooperating
	%quotient = %z / $RFW::WorldBasisSizeZActual;
	%quotient-= mFloor(%quotient);
	return mFloatLength(%quotient * $RFW::WorldBasisSizeZActual, 1);
}

function fxDtsBrick::RFW_KillBrick(%this, %sourceObject)
{
	if(isEventPending(%this.delSched))
	{
		return;
	}
	%this.hp = 0;
	%this.schedule(0, fakeKillBrick, "0 0 10", 1);
	%this.setRaycasting(false);
	%this.setColliding(false);
	%this.delSched = %this.schedule(1000, delete);
	if(%this.getGroup().bl_id == 888888)
	{
		%data = %this.getDatablock();
		for(%i = 0; %i < %this.getNumUpBricks(); %i++)
		{
			%o = %this.getUpBrick(%i);
			if(%o.getDatablock().isCube || %o.HPmult || isEventPending(%o.delSched))
			{
				continue;
			}
			%o.delSched = %o.schedule(250, damage, %sourceObject, "INF");
			for(%j = 0; %j < %o.getNumDownBricks(); %j++)
			{
				%o2 = %o.getDownBrick(%j);
				if(isObject(%o2) && !isEventPending(%o2.delSched))
				{
					cancel(%o.delSched);
					break;
				}
			}
		}
	}
}

function fxDtsBrick::Damage(%this, %sourceObject, %amt, %dmgTier, %pos, %normal)
{
	if(isEventPending(%this.delSched))
	{
		return;
	}
	if(isObject(%sourceObject) && getTrustLevel(%sourceObject, %this) >= 2)
	{
		//instantly break
		%amt = "INF";
	}
	%isPublic = %this.getGroup().bl_id == 888888;
	//find nearest totem
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
	if(!isObject(%closestTotem) || %this.getDatablock().uiName $= "Totem" || (isObject(%sourceObject) && getTrustLevel(%sourceObject, %closestTotem) >= 2))
	{
		%totem = 0;
	}
	else
	{
		%totem = %closestTotem;
	}
	%material = $RFW::ResourceByColor[%this.colorID];
	if(%this.hp $= "")
	{
		%hp = %this.getVolume() * $RFW::ResourceQuality[%material];
		if(isObject(%totem))
		{
			%hp*= %totem.HPmult;
		}
		%this.hp = %hp;
		%this.maxHp = %hp;
	}
	if(isObject(%totem))
	{
		%dmgTier-= %totem.tierBonus;
	}
	if(%amt !$= "INF" && $RFW::ResourceTier[%material] == (%dmgTier + 1))
	{
		%dmgTier++;
		%amt/= 2;
	}
	if($RFW::ResourceTier[%material] <= %dmgTier || %amt $= "INF")
	{
		if(!%isPublic || %this.getNumUpBricks() == 0 || %this.getDatablock() != nameToID(brick16x16fdata))
		{
			%this.hp-= %amt;
		}
		if(%amt > 0)
		{
			cancel(%this.resetSched);
			%this.resetSched = %this.schedule(120000, resetHP);
			if(isObject(%totem))
			{
				if(isObject(%sourceObject) && %sourceObject.getClassName() $= "Player")
				{
					%sourceObject.damage(%sourceObject, %pos, %totem.damageReflection * %amt, $DamageType::Totem);
				}
				if(!isEventPending(%this.HPregenSched) && %totem.HPregen)
				{
					%this.HPregenSched = %this.HPregenSched(%totem.HPregen);
				}
			}
		}
		switch$(%material)
		{
			case "Plant matter":
				%sound = "plantHitSound";
			case "Snow":
				%sound = "sandHitSound" @ getRandom(1, 3);
			case "Sand":
				%sound = "sandHitSound" @ getRandom(1, 3);
			case "Stone":
				%sound = "stoneHitSound" @ getRandom(1, 5);
			case "Flesh":
				%sound = "fleshHitSound" @ getRandom(1, 3);
			case "Surtra":
				%sound = "metalHitSound";
			case "Calcite":
				%sound = "metalHitSound";
			case "Sanguite":
				%sound = "metalHitSound";
			case "Nithlite":
				%sound = "metalHitSound";
			case "Sanguite":
				%sound = "metalHitSound";
			case "Glass":
				%sound = "glassHitSound";
			default:
				%sound = "dirtHitSound" @ getRandom(1, 7);
		}
		serverPlay3D(%sound, %pos);
	}
	else
	{
		serverPlay3D(metalHitSound, %pos);
		%sourceObject.addVelocity(vectorAdd(vectorScale(%normal, 8), "0 0 8"));
	}
	if(%this.hp <= 0 || %amt $= "INF")
	{
		%this.RFW_KillBrick(%sourceObject);
		//give killer resources
		if(isObject(%sourceObject))
		{
			%client = %sourceObject.getClassName() $= "GameConnection" ? %sourceObject : %sourceObject.client;
			if(isObject(%client))
			{
				%material = $RFW::ResourceByColor[%this.colorID];
				%quantity = %this.getVolume();
				%itemDatum = %material TAB %quantity TAB 0;
				%client.account.addItem(%itemDatum, true);
				if(isEventPending(%this.unburnSched))
				{
					if(%this.emitter.emitter $= "FaeFireExplosionEmitter")
					{
						%client.account.addItem("Faerie dust\t3\t0", true);
					}
					else if(%this.emitter.emitter $= "FaeFireTrailEmitter")
					{
						%client.account.addItem("Faerie dust\t1\t0", true);
					}
				}
			}
			if(%sourceObject.getClassName() $= "Player")
			{
				%sourceObject.updateBottomPrintHUD("Digging", %material TAB 0 TAB %this.maxHp);
			}
		}
	}
	else
	{
		%data = %this.getDatablock();
		if(%isPublic && %data.isCube && %data.brickSizeX > 4)
		{
			%mult = %data.brickSizeX == 16 ? 0.984375 : 0.875;
			if(%this.hp <= %mult * %this.maxHP)
			{
				%material = $RFW::ResourceByColor[%this.colorID];
				%itemDatum = %material TAB 8 TAB 0;
				if(%data == $RFW::WorldBasis)
				{
					AddAdjacentBricks(%this.getPosition(), %this.underground);
				}
				if(%sourceObject.getClassName() $= "Player")
				{
					%sourceObject.client.account.addItem(%itemDatum, true);
					%sourceObject.updateBottomPrintHUD("Digging", %material TAB 0 TAB %this.maxHp);
					%sourceObject.schedule(0, addVelocity, "0 0 2");
				}
				else
				{
					%sourceObject.account.addItem(%itemDatum, true);
				}
				%this.chisel(%pos, 4, $PublicClient, %sourceObject);
				return;
			}
		}
		if(%sourceObject.getClassName() $= "Player")
		{
			%sourceObject.updateBottomPrintHUD("Digging", %material TAB %this.hp TAB %this.maxHp);
		}
	}
}

function fxDtsBrick::resetHP(%this)
{
	cancel(%this.resetSched);
	%this.hp = "";
	%this.maxhp = "";
}

function fxDtsBrick::HPregenSched(%this, %amt)
{
	cancel(%this.HPregenSched);
	%this.hp = mClampF(%this.hp + %amt, 0, %this.maxhp);
	if(%this.hp == %this.maxHP)
	{
		return;
	}
	%this.HPregenSched = %this.schedule(1000, HPregenSched, %amt);
}

function AddAdjacentBricks(%pos0, %underground)
{
	%offsetMatrix = "1 0 0\n-1 0 0\n0 1 0\n0 -1 0\n0 0 -1";
	if(%underground)
	{
		%offsetMatrix = %offsetMatrix NL "0 0 1";
	}
	%rc = getRecordCount(%offsetMatrix);
	$RFW::Tunneled[%pos0] = true;
	%z0 = getWord(%pos0, 2);
	%offset0 = RFW_GetOffset(%z0);
	%x = $RFW::WorldBasisSizeX;
	%masks = $Typemasks::PlayerObjectType | $Typemasks::FxBrickAlwaysObjectType;
	for(%i = 0; %i < %rc; %i++)
	{
		%dir = getRecord(%offsetMatrix, %i);
		if(%dir $= %normal)
		{
			continue;
		}
		%pos1 = vectorAdd(%pos0, vectorScale(%dir, %x));
		%hit = containerFindFirst(%masks, %pos1, 0.1, 0.1, 500);
		%highestZ = 0; //bricks cannot be placed below z = 0
		while(isObject(%hit))
		{
			if(%hit.getGroup().bl_id == 888888 && %hit.getDatablock() == $RFW::WorldBasis && (%z = getWord(%hit.getPosition(), 2)) > %highestZ)
			{
				%highestZ = %z;
				%highest = %hit;
			}
			%hit = containerFindNext();
		}
		//if the highest Z is still 0 we're either at the edge of the map or someone has mined all the way to the bottom...
		if(%highestZ != 0)
		{
			if(%highestZ < %z0)
			{
				continue;
			}
			%offset1 = RFW_GetOffset(%highestZ);
			if(%offset0 == %offset1)
			{
				//same offset, just need one brick here
				//check that this area is empty and this area hasn't already been tunneled through
				if(!ContainerBoxEmpty(%masks, %pos1, %x / 2 - 0.1, %x / 2 - 0.1, $RFW::WorldBasisSizeZActual / 2 - 0.1) || $RFW::Tunneled[%pos1])
				{
					continue;
				}
				$RFW::Tunneled[%pos1] = true;
				AddBasisBlock(%pos1, $RFW::ResourceByColor[%highest.colorID]);
			}
			else
			{
				//different offsets, need two bricks
				%z1 = %z0 - %offset0 + %offset1;
				%nPos0 = setWord(%pos1, 2, %z1);
				if(%z1 < %z0)
				{
					%nPos1 = vectorAdd(%nPos0, "0 0 " @ $RFW::WorldBasisSizeZActual);
				}
				else
				{
					%nPos1 = vectorSub(%nPos0, "0 0 " @ $RFW::WorldBasisSizeZActual);
				}
				//first brick
				if(!ContainerBoxEmpty(%masks, %nPos0, %x / 2 - 0.1, %x / 2 - 0.1, $RFW::WorldBasisSizeZActual / 2 - 0.1) || $RFW::Tunneled[%nPos0])
				{
					//continue;
				}
				else
				{
					$RFW::Tunneled[%nPos0] = true;
					AddBasisBlock(%nPos0, $RFW::ResourceByColor[%highest.colorID]);
				}
				//second brick
				if(!ContainerBoxEmpty(%masks, %nPos1, %x / 2 - 0.1, %x / 2 - 0.1, $RFW::WorldBasisSizeZActual / 2 - 0.1) || $RFW::Tunneled[%nPos1])
				{
					continue;
				}
				$RFW::Tunneled[%nPos1] = true;
				AddBasisBlock(%nPos1, $RFW::ResourceByColor[%highest.colorID]);
			}
		}
	}
}

function AddBasisBlock(%pos, %mat)
{
	%brick = new fxDtsBrick()
	{
		datablock = $RFW::WorldBasis;
		position = %pos;
		colorID = $RFW::ResourceColor[%mat];
		colorFxID = $RFW::ResourceColorFX[%mat];
		isPlanted = true;
		underground = true;
	};
	%err = %brick.plant();
	if(!%err || %err == 2)
	{
		Brickgroup_888888.add(%brick);
		%brick.plantedTrustCheck();
	}
	else
	{
		%brick.delete();
		return;
	}
	%oreCt = $RFW::Ores[%mat];
	%roll = getRandom();
	if(%oreCt > 0 && !$OresDepleted)
	{
		$OresDepleted = true;
		schedule(100, 0, eval, "$OresDepleted = false;");
		%roll = getRandom();
		for(%i = 0; %i < %oreCt; %i++)
		{
			%ore = $RFW::Ore[%mat, %i];
			if(getRandom() < getWord(%ore, 0))
			{
				AddAdjacentBricks(%pos, %brick.underground);
				%x = getWord(%pos, 0) + 2 * (getRandom() - 0.5) * $RFW::WorldBasisSizeX;
				%y = getWord(%pos, 1) + 2 * (getRandom() - 0.5) * $RFW::WorldBasisSizeX;
				%z = getWord(%pos, 2) + 2 * (getRandom() - 0.5) * $RFW::WorldBasisSizeZActual;
				%brick.addOre(%x SPC %y SPC %z, 4, $PublicClient, getWord(%ore, 1));
				break;
			}
		}
	}
}

function fxDtsBrick::addOre(%this, %pos, %size, %client, %ore)
{
	%data = %this.getDatablock();
	%name = %data.getName();
	if(%name !$= "brick64xCubeData" && %name !$= "brick32xCubeData" && %name !$= "brick16xCubeData" && %name !$= "brick8xCubeData" && %name !$= "brick4xCubeData" && %name !$= "brick2xCubeData")
	{
		return;
	}
	else if((%size != 2 || !isObject(brick2xCubeData)) && %size != 4 && %size != 8 && %size != 16 && %size != 32 && %size != 64)
	{
		return;
	}
	else
	{
		%srcSize = %data.brickSizeX;
		if(%srcSize <= %size)
		{
			%this.setColor($RFW::ResourceColor[%ore]);
			%this.setColorFx($RFW::ResourceColorFx[%ore]);
			return;
		}
		%bricks = %this.fracture(%client);
		%closest = -1;
		%minDist = 9999;
		for(%i = 0; %i < getWordCount(%bricks); %i++)
		{
			if(!isObject(%brick = getWord(%bricks, %i)))
			{
				continue;
			}
			%dist = vectorDist(%pos, %brick.getPosition());
			// if(%dist < %minDist)
			// {
				// %minDist = %dist;
				// %closest = %brick;
			// }
			if(getRandom() < 0.33)
			{
				%brick.addOre(%pos, %size, %client, %ore);
			}
		}
		// if(isObject(%closest))
		// {
			// %closest.addOre(%pos, %size, %client, %ore);
		// }
	}
}

function fxDtsBrick::chisel(%this, %pos, %size, %client, %sourceObject)
{
	%data = %this.getDatablock();
	%name = %data.getName();
	if(%name !$= "brick64xCubeData" && %name !$= "brick32xCubeData" && %name !$= "brick16xCubeData" && %name !$= "brick8xCubeData" && %name !$= "brick4xCubeData" && %name !$= "brick2xCubeData")
	{
		return;
	}
	else if((%size != 2 || !isObject(brick2xCubeData)) && %size != 4 && %size != 8 && %size != 16 && %size != 32 && %size != 64)
	{
		return;
	}
	else
	{
		%srcSize = %data.brickSizeX;
		if(%srcSize <= %size)
		{
			%this.schedule(1, RFW_KillBrick, %sourceObject);
			//%this.delete();
			return;
		}
		%bricks = %this.fracture(%client);
		%closest = -1;
		%minDist = 9999;
		for(%i = 0; %i < getWordCount(%bricks); %i++)
		{
			if(!isObject(%brick = getWord(%bricks, %i)))
			{
				continue;
			}
			%dist = vectorDist(%pos, %brick.getPosition());
			if(%dist < %minDist)
			{
				%minDist = %dist;
				%closest = %brick;
			}
		}
		if(isObject(%closest))
		{
			%closest.chisel(%pos, %size, %client, %sourceObject);
		}
	}
}

function SAG_BrickPlantStuff(%brick, %client)
{
	if(!isObject(%brick))
	{
		//already deleted, perhaps by the chisel
		return -1;
	}
	%err = %brick.plant();
	%client.brickGroup.add(%brick);
	%brick.plantedTrustCheck();
	return %err;
}

function fxDtsBrick::fracture(%this, %client)
{
	%data = %this.getDatablock();
	%name = %data.getName();
	if(%name !$= "brick64xCubeData" && %name !$= "brick32xCubeData" && %name !$= "brick16xCubeData" && %name !$= "brick8xCubeData" && (%name !$= "brick4xCubeData" || !isObject(brick2xCubeData)))
	{
		return;
	}
	else
	{
		%basepos = %this.getPosition();
		%color = %this.colorID;
		%colorFx = %this.colorFxID;
		%cl = isObject(%client) ? %client : %this.client;
		
		%this.delete();
		%newsize = %data.brickSizeX / 2;
		%newdata = "Brick" @ %newsize @ "xCubeData";
		%offset = %newsize / 4;
		%offsetMatrix = "-1 -1 -1\t1 -1 -1\t-1 1 -1\t1 1 -1\t-1 -1 1\t1 -1 1\t-1 1 1\t1 1 1";
		%bricklist = "";
		for(%i = 0; %i < getFieldCount(%offsetMatrix); %i++)
		{
			%pos = vectorAdd(%basepos, vectorScale(getField(%offsetMatrix, %i), %offset));
			%brick = new fxDtsBrick()
			{
				datablock = %newdata;
				position = %pos;
				colorID = %color;
				colorFxID = %colorFx;
				shapeFxID = 0;
				angleID = 0;
				client = %cl;
				stackBL_ID = %cl.BL_ID; 
				isPlanted = true;
			};
			//deleted bricks no longer instantly stop causing overlaps wth new bricks, so schedule this brick's actual planting
			schedule(1, 0, SAG_BrickPlantStuff, %brick, %cl);
			%bricklist = %bricklist SPC %brick;
		}
	}
	return %bricklist;
}