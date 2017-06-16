if($GameModeArg !$= "Add-Ons/Gamemode_ReverieFortWars/gamemode.txt" && !brickVehicleSpawnData.initBrickRev)
{
	brickVehicleSpawnData.initBrickRev = 1;
	forceRequiredAddOn("Ai_Mutalid");
	forceRequiredAddOn("Brick_2x_Cube");
	//CRC'd -- why?
	exec("add-ons/Brick_Corners/server.cs");
	forceRequiredAddOn("Brick_Large_Cubes");
	forceRequiredAddOn("Brick_Treasure_Chest");
	forceRequiredAddOn("Vehicle_AdvancedHorse");
}

if(!brickVehicleSpawnData.initRev)
{
	brickVehicleSpawnData.initRev = 1;

	//$Pref::Server::GhostLimit = 1280000;
	//$Pref::Server::BrickLimit = 1048576;
	$GameModeDisplayName = "Tribal RPG - v0.9";
	$Pref::Server::Name = "RFW";

	$CorpseTimeoutValue = 60000;
	$Game::MinRespawnTime = 15000;

	$Pref::Server::AS_["BootLoad"] = 0;

	exec("./scripts/leveling.cs");
	exec("./scripts/geometry.cs");
	exec("./scripts/resources.cs");
	exec("./scripts/datablocks.cs");
	exec("./scripts/combat.cs");
	exec("./scripts/accounts.cs");
	exec("./scripts/digging.cs");
	exec("./scripts/items.cs");
	exec("./scripts/appearance.cs");
	exec("./scripts/crafting.cs");
	exec("./scripts/interfacing.cs");
	exec("./scripts/BrickControlsMenu.cs");
	exec("./scripts/tribes.cs");
	exec("./scripts/building.cs");
	exec("./scripts/totems.cs");
	exec("./scripts/help.cs");
	exec("./scripts/notice.cs");
	exec("./scripts/statusEffects.cs");
	exec("./scripts/Support_InventorySlotAdjustment.cs");
	exec("./scripts/hitboxes.cs");
	exec("./scripts/reverieAI.cs");
	exec("./scripts/sunrise.cs");
	exec("./scripts/footsteps.cs");
	exec("./scripts/chests.cs");
	exec("./scripts/buildWorld.cs");
}

//todo:
//random level downs
//random items disappearing
//fix RAM usage

//return; //testing

// PlayerStandardArmor.isInvincible = true; //for admin (ab)use

function RFW_Init(%bypass)
{
	echo("--- BRICK LIMIT: " @ getBrickLimit());
	echo("--- GHOST LIMIT: " @ getGhostLimit());
	brickVehicleSpawnData.uiName = "";
	brickVehicleSpawnData.category = "";
	brickVehicleSpawnData.subcategory = "";

	brick8xWaterRiverData.uiName = "";
	brick8xWaterRiverData.category = "";
	brick8xWaterRiverData.subcategory = "";

	brick8xWaterRapidsData.uiName = "";
	brick8xWaterRapidsData.category = "";
	brick8xWaterRapidsData.subcategory = "";

	brick8xWaterData.uiName = "";
	brick8xWaterData.category = "";
	brick8xWaterData.subcategory = "";

	brick32xWaterData.uiName = "";
	brick32xWaterData.category = "";
	brick32xWaterData.subcategory = "";

	createUINameTable();

	if(!$RFW::Init || %bypass)
	{
		if($Server::ServerType !$= "SinglePlayer")
		{
			if(isFile("saves/RFW.bls"))
			{
				//load as public

				if(isFile("saves/AutoSaver/ReverieFortWars.bls") && isFunction("Autosaver_Begin"))
					schedule(100, 0, ServerDirectSaveFileLoad, "saves/AutoSaver/ReverieFortWars.bls", 3, "", 2);
				else
					schedule(100, 0, ServerDirectSaveFileLoad, "saves/RFW.bls", 3, "", 2);
			}
			else if(isfile("saves/RFW_Backup.bls"))
			{
				//load with saved owner
				schedule(100, 0, ServerDirectSaveFileLoad, "saves/RFW_Backup.bls", 3, "", 1);
			}
			else
				schedule(100, 0, ReverieFortWars_BuildWorld);
		}
	}

	$RFW::Init = 1;
}
schedule(0, 0, "RFW_Init");

function RFW_Autosave()
{
	cancel($RFW_Autosave);
	if($Server::ServerType !$= "SinglePlayer")
	{
		export("$RFW::Tribe*", "config/saves/RFW/Tribes.cs");
		export("$RFW::Chest*", "config/saves/RFW/Chests.cs");
		if(isFunction("Autosaver_Begin"))
			Autosaver_Begin("ReverieFortWars");
		else
			RFW_SaveBricks(0, 0);
	}

	if(clientGroup.getCount() > 0)
	{
		%clientCount = clientGroup.getCount();
		for(%i = 0; %i < %clientCount; %i++)
		{
			%client = clientGroup.getObject(%i);
			%acc = %client.account;
			%acc.setName("NewlyLoaded");
			%acc.save("config/server/RFW/" @ %client.bl_id @ ".cs");
			%acc.setName("");
		}
		echo("RFW: Autosave complete.");
	}
	$RFW_Autosave = schedule(120000, 0, RFW_Autosave);
}
if(!isEventPending($RFW_Autosave))
	$RFW_Autosave = schedule(120000, 0, RFW_Autosave);

function RFW_SaveBricks(%i, %j)
{
	if(!isObject($RFW::FileObject))
	{
		$RFW::FileObject = new FileObject();
		$RFW::FileObject.openForWrite("saves/RFW_Backup.bls");
		$RFW::FileObject.writeLine("This is a Blockland save file.  You probably shouldn't modify it cause you'll screw it up.");
		$RFW::FileObject.writeLine("1");
		$RFW::FileObject.writeLine("");
		for(%c = 0; %c < 64; %c++)
		{
			$RFW::FileObject.writeLine(getColorIdTable(%c));
		}
		$RFW::FileObject.writeLine("Linecount " @ getBrickCount()); //pretty sure this doesn't do anything
	}
	if(mainBrickGroup.getCount() <= %i)
	{
		$RFW::FileObject.close();
		$RFW::FileObject.delete();
		echo("RFW: Bricks autosave complete.");
		return;
	}
	%group = mainBrickGroup.getObject(%i);
	%groupCount = %group.getCount();
	while(%iter++ < 100)
	{
		if(%j >= %groupCount)
		{
			return schedule(0, 0, RFW_SaveBricks, %i + 1, 0);
		}
		%brick = %group.getObject(%j);
		$RFW::FileObject.writeLine(%brick.getDatablock().uiName @ "\" " @ %brick.getPosition() SPC %brick.angleID SPC 1 SPC %brick.colorID SPC (%brick.printID == 0 ? "" : %brick.printID) SPC %brick.colorFx SPC %brick.shapeFX SPC %brick.isRaycasting() SPC %brick.isColliding() SPC %brick.isRendering());
		$RFW::FileObject.writeLine("+-OWNER " @ %group.bl_id);
		if(isObject(%light = %brick.light))
		{
			$RFW::FileObject.writeLine("+-LIGHT " @ %light.getDatablock().uiName @ "\" 1"); //what's this 1 for?
		}
		if(isObject(%emitter = %brick.emitter))
		{
			$RFW::FileObject.writeLine("+-EMITTER " @ %emitter.emitter.uiName @ "\" " @ %brick.emitterDirection);
		}
		if(%eventCount = %brick.numEvents)
		{
			for(%e = 0; %e < %eventCount; %e++)
			{
				%ln = "+-EVENT" TAB %e TAB %brick.eventEnabled[%e] TAB %brick.eventInput[%e] TAB %brick.eventDelay[%e] TAB %brick.eventTarget[%e] TAB %brick.eventNT[%e] TAB %brick.eventOutput[%e];
				for(%f = 1; %f < 5; %f++)
				{
					%ln = %ln TAB %brick.eventOutputParameter[%e, %f];
				}
				$RFW::FileObject.writeLine(%ln);
			}
		}
		%j++;
	}
	return schedule(1, 0, RFW_SaveBricks, %i, %j);
}

function shutdown(%time)
{
	if(%time <= 0)
	{
		%time = 60;
	}
	messageAll('MsgError', "\c4SERVER SHUTDOWN IN " @ %time @ " SECONDS");
	echo("SERVER SHUTDOWN IN " @ %time @ " SECONDS");
	RFW_Autosave();
	cancel($RFW_Autosave);
	schedule(%time * 1000, 0, shutdown_exec);
}

function shutdown_exec()
{
	if(isObject($RFW::FileObject))
	{
		return schedule(1000, 0, shutdown_exec);
	}
	quit();
}

//function MingiameSO::ForceEquip()
//{
//
//}

//no-good rotten tricksters
function serverCmdCreateMinigame()
{
}

function serverCmdWand()
{
}

if(isPackage("ReverieFortWars"))
	deactivatePackage("ReverieFortWars");

package ReverieFortWars
{
	function Armor::onEnterLiquid(%data, %obj, %coverage, %type)
	{
		Parent::onEnterLiquid(%data, %obj, %coverage, %type);
		if(%type != 0)
		{
			%obj.invulnerable = false;
			%obj.damage(%obj, %obj.getPosition(), 10000, $DamageType::Lava);
		}
	}

	function GameConnection::createPlayer(%client, %transform)
	{
		%client.schedule(0, "ApplyHealthBar");
		return Parent::createPlayer(%client, %transform);
	}
	
	function GameConnection::spawnPlayer(%client)
	{
		%result = parent::spawnPlayer(%client);
		%player = %client.player;	
		if(isObject(%player))
			%player.RFW_DisplayHealth();
		
		return %result;
	}

	function Armor::onDisabled(%this, %obj)
	{
		%obj.RFW_DisplayHealth();
		return Parent::onDisabled(%this, %obj);
	}
	
	function Armor::onNewDatablock(%this, %obj, %b)
	{
		Parent::onNewDatablock(%this, %obj, %b);
		%obj.RFW_DisplayHealth();
	}

	// onDamage also handles negative integers.
	function Armor::onDamage(%this, %obj, %damage)
	{
		parent::onDamage(%this, %obj, %damage);
		%obj.RFW_DisplayHealth();
	}
};
activatePackage("ReverieFortWars");

function Player::RFW_DisplayHealth(%this)
{
	if(%this.getClassName() $= "AIPlayer")
		%name = %this.type.name;
	else
	{
		if(isObject(%cl = %this.client))
		{
			if(isObject(%acc = %cl.account))
			{
				if(%acc.vLevel !$= "")
					%lvPr = "[" @ %acc.vLevel @ "] ";
			}

			%name = %lvPr @ %cl.getPlayerName();
		}
		else
			%name = "";
	}

	if(%this.getState() $= "dead")
		%this.SetPlayerShapeName("");	
	else
		%this.SetPlayerShapeName(%name @ "(" @ mFloatLength(%this.getHealth() / %this.getMaxHealth() * 100, 1) @ "%)");
}

function Player::SetPlayerShapeName(%this,%name){%this.setShapeName(%name,"8564862");}
function AIPlayer::SetPlayerShapeName(%this,%name,%tog){if(%tog && trim(%name) !$= "") %name = "(AI) " @ %name; %this.setShapeName(%name,"8564862");}