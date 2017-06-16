//ReverieFortWars/combat.cs

//TABLE OF CONTENTS
// #0. deaath message declarations
// #1.
// #1.1 assists functionality
// #2. package

// #0.
AddDamageType("Sword", '<bitmap:Add-Ons/Weapon_Sword/CI_sword> %1', '%2 <bitmap:Add-Ons/Weapon_Sword/CI_sword> %1', 0.75, 1);
AddDamageType("Pickaxe", '<bitmap:Add-Ons/Weapon_Sword/CI_sword> %1', '%2 <bitmap:Add-Ons/Weapon_Sword/CI_sword> %1', 0.75, 1);
AddDamageType("Dagger", '<bitmap:Add-Ons/Weapon_Sword/CI_sword> %1', '%2 <bitmap:Add-Ons/Weapon_Sword/CI_sword> %1', 0.75, 1);
AddDamageType("Bow", '<bitmap:add-ons/Weapon_Bow/CI_arrow> %1', '%2 <bitmap:add-ons/Weapon_Bow/CI_arrow> %1', 0.5, 1);
AddDamageType("FaeFire", '<bitmap:Add-Ons/Weapon_Sword/CI_sword> %1', '%2 <bitmap:Add-Ons/Weapon_Sword/CI_sword> %1', 1, 1);
AddDamageType("Totem", '<bitmap:Add-Ons/Weapon_Sword/CI_sword> %1', '%2 <bitmap:Add-Ons/Weapon_Sword/CI_sword> %1', 1, 1);
AddDamageType("Lens", '<bitmap:Add-Ons/Weapon_Sword/CI_sword> %1', '%2 <bitmap:Add-Ons/Weapon_Sword/CI_sword> %1', 1, 1);
AddDamageType("Mclaw", '<bitmap:Add-Ons/Weapon_Sword/CI_sword> %1', '%2 <bitmap:Add-Ons/Weapon_Sword/CI_sword> %1', 1, 1);
AddDamageType("OssiStaff", '<bitmap:Add-Ons/Weapon_Sword/CI_sword> %1', '%2 <bitmap:Add-Ons/Weapon_Sword/CI_sword> %1', 1, 1);
AddDamageType("Pyromancy", '<bitmap:Add-Ons/Weapon_Sword/CI_sword> %1', '%2 <bitmap:Add-Ons/Weapon_Sword/CI_sword> %1', 1, 1);
AddDamageType("SanguiWand", '<bitmap:Add-Ons/Weapon_Sword/CI_sword> %1', '%2 <bitmap:Add-Ons/Weapon_Sword/CI_sword> %1', 1, 1);
AddDamageType("Bleed", '<bitmap:Add-Ons/Weapon_Sword/CI_sword> %1', '%2 <bitmap:Add-Ons/Weapon_Sword/CI_sword> %1', 1, 1);

$RFW::DeathMessageCount[$DamageType::Default] = 4;
$RFW::DeathMessage[$DamageType::Default, 0] = "%1 put %2 out of their misery";
$RFW::DeathMessage[$DamageType::Default, 1] = "%1 put an end to %2";
$RFW::DeathMessage[$DamageType::Default, 2] = "%1 was the last thing %2 ever saw";
$RFW::DeathMessage[$DamageType::Default, 3] = "%1 was the end of %2";
$RFW::DeathMessage[$DamageType::Default, 4] = "%1 murdered %2";
$RFW::DeathMessageCount[$DamageType::Sword] = 4;
$RFW::DeathMessage[$DamageType::Sword, 0] = "%1 ran %2 through";
$RFW::DeathMessage[$DamageType::Sword, 1] = "%1 sliced %2 to ribbons";
$RFW::DeathMessage[$DamageType::Sword, 2] = "%1 chopped %2 into pieces";
$RFW::DeathMessage[$DamageType::Sword, 3] = "%1 removed %2's head";
$RFW::DeathMessage[$DamageType::Sword, 4] = "%1 found out what %2 was really made of";
$RFW::DeathMessageCount[$DamageType::Dagger] = 4;
$RFW::DeathMessage[$DamageType::Dagger, 0] = "%1 poked %2 full of holes";
$RFW::DeathMessage[$DamageType::Dagger, 1] = "%1 stuck %2 with the pointy end";
$RFW::DeathMessage[$DamageType::Dagger, 2] = "%1 stabbed %2 in the back";
$RFW::DeathMessage[$DamageType::Dagger, 3] = "%1 cut the life from %2";
$RFW::DeathMessage[$DamageType::Dagger, 4] = "%1 examined %2's insides";
$RFW::DeathMessageCount[$DamageType::Pickaxe] = 4;
$RFW::DeathMessage[$DamageType::Pickaxe, 0] = "%1 impaled %2";
$RFW::DeathMessage[$DamageType::Pickaxe, 1] = "%1 mistook %2 for a rock";
$RFW::DeathMessage[$DamageType::Pickaxe, 2] = "%1 mined out %2's skull";
$RFW::DeathMessage[$DamageType::Pickaxe, 3] = "%1 axed %2 a question";
$RFW::DeathMessage[$DamageType::Pickaxe, 4] = "%1 really digs %2";
$RFW::DeathMessageCount[$DamageType::Bow] = 3;
$RFW::DeathMessage[$DamageType::Bow, 0] = "%1 filled %2 with arrows";
$RFW::DeathMessage[$DamageType::Bow, 1] = "%1 used %2 for target practice";
$RFW::DeathMessage[$DamageType::Bow, 2] = "%1 couldn't teach %2 how to dodge arrows";
$RFW::DeathMessage[$DamageType::Bow, 3] = "%1 turned %2 into a pincushion";
$RFW::DeathMessageCount[$DamageType::Snowball] = 2;
$RFW::DeathMessage[$DamageType::Snowball, 0] = "%1 built a snowman out of %2";
$RFW::DeathMessage[$DamageType::Snowball, 1] = "%1 gave %2 a brainfreeze";
$RFW::DeathMessage[$DamageType::Snowball, 2] = "%1 snowballed %2";
$RFW::DeathMessageCount[$DamageType::FaeFire] = 4;
$RFW::DeathMessage[$DamageType::FaeFire, 0] = "A faerie burned %2 to an eldritch crisp";
$RFW::DeathMessage[$DamageType::FaeFire, 1] = "A faerie incinerated %2";
$RFW::DeathMessage[$DamageType::FaeFire, 2] = "A faerie cremated %2";
$RFW::DeathMessage[$DamageType::FaeFire, 3] = "A faerie blasted %2 to cinders";
$RFW::DeathMessage[$DamageType::FaeFire, 4] = "A faerie transmuted %2 into eldritch ash";
$RFW::DeathMessageCount[$DamageType::Lava] = 3;
$RFW::DeathMessage[$DamageType::Lava, 0] = "%2 took a lava bath";
$RFW::DeathMessage[$DamageType::Lava, 1] = "%2 can't swim in lava";
$RFW::DeathMessage[$DamageType::Lava, 2] = "%2 couldn't take the heat";
$RFW::DeathMessage[$DamageType::Lava, 3] = "%2 is sleeping with the lava fishies";
$RFW::DeathMessageCount[$DamageType::Suicide] = 1;
$RFW::DeathMessage[$DamageType::Suicide, 0] = "%2 couldn't handle the pressure";
$RFW::DeathMessage[$DamageType::Suicide, 1] = "%2 took the easy way out";
$RFW::DeathMessageCount[$DamageType::Fall] = 2;
$RFW::DeathMessage[$DamageType::Fall, 0] = "%2 fell down a well";
$RFW::DeathMessage[$DamageType::Fall, 1] = "%2 broke every bone in their body";
$RFW::DeathMessage[$DamageType::Fall, 2] = "%2 didn't stick the landing";
$RFW::DeathMessageCount[$DamageType::Totem] = 2;
$RFW::DeathMessage[$DamageType::Totem, 0] = "%2 picked the wrong fort to raid";
$RFW::DeathMessage[$DamageType::Totem, 1] = "%2 succumbed to a curse";
$RFW::DeathMessage[$DamageType::Totem, 2] = "In Soviet Russia, fort destroys %2";
$RFW::DeathMessageCount[$DamageType::Mclaw] = 3;
$RFW::DeathMessage[$DamageType::Mclaw, 0] = "%1 devoured %2";
$RFW::DeathMessage[$DamageType::Mclaw, 1] = "%1 ripped %2 limb from limb";
$RFW::DeathMessage[$DamageType::Mclaw, 2] = "%1 tore %2 in half";
$RFW::DeathMessage[$DamageType::Mclaw, 3] = "%1 feasted upon %2";
$RFW::DeathMessageCount[$DamageType::Lens] = 2;
$RFW::DeathMessage[$DamageType::Lens, 0] = "%1 blinded %2";
$RFW::DeathMessage[$DamageType::Lens, 1] = "%1 enlightened %2";
$RFW::DeathMessage[$DamageType::Lens, 2] = "%1 showed %2 the light";
$RFW::DeathMessageCount[$DamageType::Bleed] = 1;
$RFW::DeathMessage[$DamageType::Bleed, 0] = "%2 succumbed to red wasting";
$RFW::DeathMessage[$DamageType::Bleed, 1] = "%2 ran out of blood";
$RFW::DeathMessageCount[$DamageType::Pyromancy] = 4;
$RFW::DeathMessage[$DamageType::Pyromancy, 0] = "%1 burned %2 to a crisp";
$RFW::DeathMessage[$DamageType::Pyromancy, 1] = "%1 incinerated %2";
$RFW::DeathMessage[$DamageType::Pyromancy, 2] = "%1 cremated %2";
$RFW::DeathMessage[$DamageType::Pyromancy, 3] = "%1 blasted %2 to cinders";
$RFW::DeathMessage[$DamageType::Pyromancy, 4] = "%1 transmuted %2 into ash";
$RFW::DeathMessageCount[$DamageType::OssiStaff] = 2;
$RFW::DeathMessage[$DamageType::OssiStaff, 0] = "%1 shattered %2's bones";
$RFW::DeathMessage[$DamageType::OssiStaff, 1] = "%1 removed %2's spine";
$RFW::DeathMessage[$DamageType::OssiStaff, 2] = "%1 transmuted %2 into calcite";
$RFW::DeathMessageCount[$DamageType::SanguiWand] = 2;
$RFW::DeathMessage[$DamageType::SanguiWand, 0] = "%1 solidified %2";
$RFW::DeathMessage[$DamageType::SanguiWand, 1] = "%1 made %2's blood into something more useful";
$RFW::DeathMessage[$DamageType::SanguiWand, 2] = "%1 transmuted %2 into sanguite";

// #1.1
function Player::AddDamager(%this, %obj)
{
	if(!isObject(%this) || !isObject(%obj))
	{
		return;
	}
	switch$(%obj.getClassName())
	{
		case "Projectile": return %this.addDamager(%this, %obj.sourceObject);
		case "Player": return %this.addDamager(%this, %obj.client);
		case "AiPlayer": return %this.addDamager(%this, %obj.type);
	}
	for(%i = 0; %i < getWordCount(%this.damagers); %i++)
	{
		if(getWord(%this.damagers, %i) $= %obj)
		{
			%this.removeDamager[%obj] = %this.schedule(10000, removeDamager, %obj);
			return;
		}
	}
	%this.damagers = %this.damagers SPC %obj;
	%this.removeDamager[%obj] = %this.schedule(10000, removeDamager, %obj);
}

function Player::RemoveDamager(%this, %obj)
{
	for(%i = 0; %i < getWordCount(%this.damagers); %i++)
	{
		if(getWord(%this.damagers, %i) $= %obj)
		{
			%this.damagers = removeWord(%this.damagers, %i);
			return;
		}
	}
}

function Player::AddAssist(%this, %obj)
{
	if(!isObject(%this) || !isObject(%obj))
	{
		return;
	}
	switch$(%obj.getClassName())
	{
		case "Projectile": return %this.addAssist(%this, %obj.sourceObject);
		case "Player": return %this.addAssist(%this, %obj.client);
		case "AiPlayer": return %this.addAssist(%this, %obj.type);
	}
	for(%i = 0; %i < getWordCount(%this.assists); %i++)
	{
		if(getWord(%this.assists, %i) $= %obj)
		{
			%this.removeAssist[%obj] = %this.schedule(10000, removeAssist, %obj);
			return;
		}
	}
	%this.assists = %this.assists SPC %obj;
	%this.removeAssist[%obj] = %this.schedule(10000, removeAssist, %obj);
}

function Player::RemoveAssist(%this, %obj)
{
	for(%i = 0; %i < getWordCount(%this.assists); %i++)
	{
		if(getWord(%this.assists, %i) $= %obj)
		{
			%this.assists = removeWord(%this.assists, %i);
			return;
		}
	}
}

function player::HpRegenSched(%this)
{
	cancel(%this.HpRegenSched);
	if(%this.isDead)
		return;

	%this.addHealth(mClampF(%this.getMaxHealth() * 0.01, 1, 100));
	//%health = mClampF(%this.getHealth() + mClampF(%this.getMaxHealth() * 0.01, 1, 100), 0, %this.getMaxHealth());
	//%this.health = %health;
	//%this.setDamageLevel(%this.getHealthLevel());

	%this.HpRegenSched = %this.schedule(6000, HpRegenSched);
}

// #1.2
function getDeathMessage(%dmgType)
{
	if(%n = $RFW::DeathMessageCount[%dmgType])
	{
		return $RFW::DeathMessage[%dmgType, getRandom(0, %n)];
	}
	else
	{
		return getTaggedString($DeathMessage_Murder[%dmgType]);
	}
}

// #1.3
$RFW::nodeList = "copHat knitHat scoutHat plume triPlume septPlume flareHelmet plumeHelmet helmet visor headSkin chest femChest shoulderPads epaulets epauletsRankA epauletsRankB epauletsRankC epauletsRankD pack armor cape quiver tank bucket lArm lArmSlim rArm rArmSlim lHand lHook rHand rHook pants skirtHip lShoe lPeg rShoe rPeg skirtTrimLeft skirtTrimRight";

function Player::Butcher(%this, %acc)
{
	if(%this.unbutcherable)
	{
		return;
	}
	serverPlay3d("fleshHitSound" @ getRandom(1, 3), %this.getPosition());
	if(%this.butcherHits < 3)
	{
		%this.butcherHits++;
		return;
	}
	if(%this.butcherHits == 3)
	{
		%this.butcherHits = 4;
		if(%this.getDatablock().shapeFile $= "base/data/shapes/player/m.dts")
		{
			%nodeCount = getWordCount($RFW::nodeList);
		}
		else
		{
			%this.setNodeColor("ALL", "0.8 0 0 1");
		}
		for(%i = 0; %i < %nodeCount; %i++)
		{
			%node = getWord($RFW::nodeList, %i);
			if(%this.isNodeVisible(%node))
			{
				%this.nodeList = %this.nodeList SPC %node;
			}
		}
		%this.nodeList = ltrim(%this.nodeList);
	}
	%r = getRandom(0, getWordCount(%this.nodeList) - 1);
	%node = getWord(%this.nodeList, %r);
	%this.nodeList = removeWord(%this.nodeList, %r);
	if(%node $= "chest" || %node $= "femChest")
	{
		%this.setDecalName("AAA-None");
		%this.setNodeColor(%node, "0.8 0 0 1");
	}
	else if(%node $= "headSkin")
	{
		%this.setFaceName("asciiTerror");
		%this.setNodeColor(%node, "0.8 0 0 1");
	}
	else if(%node $= "pants" || %node $= "skirtHip")
	{
		%this.setNodeColor(%node, "0.8 0 0 1");
	}
	else if(%node $= "copHat" || %node $= "knitHat" || %node $= "scoutHat" || %node $= "plume" || %node $= "triPlume" || %node $= "septPlume" || %node $= "flareHelmet" || %node $= "plumeHelmet" || %node $= "helmet" || %node $= "visor" || %node $= "pack" || %node $= "cape" || %node $= "armor" || %node $= "quiver" || %node $= "tank" || %node $= "bucket")
	{
		%this.hideNode(%node);
	}
	else if(%node $= "lHook" || %node $= "rHook" || $node $= "lPeg" || %node $= "rPeg")
	{
		%this.hideNode(%node);
		%acc.addItem("Wood\t1\t0", true);
		return;
	}
	else
	{
		%this.hideNode(%node);
	}
	%loot = %this.butcherLoot;
	if(getFieldCount(%loot) == 0)
	{
		if(!strLen(%this.loot[0]) && getWordCount(%this.nodeList) == 0)
		{
			if(%this.finalButchery++ == 3)
			{
				%this.spawnExplosion(deathProjectile, getWord(%this.getScale(), 2));
				%this.delete();
			}
		}
		return;
	}
	%r = getRandom(0, getFieldCount(%loot) - 1);
	%lootField = getField(%loot, %r);
	%rem = firstWord(%lootField);
	%amt = getRandom(1, %rem);
	%rem = %rem - %amt;
	%lootField = setWord(%lootField, 0, %rem);
	if(%rem > 0)
	{
		%this.butcherLoot = setField(%loot, %r, %lootField);
	}
	else
	{
		%this.butcherLoot = removeField(%loot, %r);
	}
	%acc.addItem(restWords(%lootField) TAB %amt TAB "", true);
}

function Player::SetButcherLoot(%obj, %flesh, %blood, %bone)
{
	if(%flesh > 0)
	{
		%flesh = %flesh SPC "Flesh";
	}
	else
	{
		%flesh = "";
	}
	if(%blood > 0)
	{
		if(%obj.sanguification > 0)
		{
			%amt = %blood * mClampF(%obj.sanguification, 0, 1);
			if(%amt < %blood)
			{
				%blood = (%blood - %amt) @ " Blood\t" @ %amt @ " Sanguite";
			}
			else
			{
				%blood = %blood SPC "Sanguite";
			}
		}
		else
		{
			%blood = %blood SPC "Blood";
		}
	}
	else
	{
		%blood = "";
	}
	if(%bone > 0)
	{
		if(%obj.ossification > 0)
		{
			%amt = %bone * mClampF(%obj.ossification, 0, 1);
			if(%amt < %bone)
			{
				%bone = (%bone - %amt) @ " Bone\t" @ %amt @ " Calcite";
			}
			else
			{
				%bone = %bone SPC "Calcite";
			}
		}
		else
		{
			%bone = %bone SPC "Bone";
		}
	}
	else
	{
		%bone = "";
	}
	%obj.butcherLoot = trim(%flesh TAB %blood TAB %bone);
}


// #2.
package RFW_MinigameDamage
{
	function minigameCanDamage(%a, %b)
	{
		%p = parent::minigameCanDamage(%a, %b);
		switch$(%a.getClassName())
		{
			case "Projectile": %a = %a.sourceObject;
			case "GameConnection": %a = %a.player;
			case "AIplayer": return true;
		}
		switch$(%b.getClassName())
		{
			case "Projectile": %b = %b.sourceObject;
			case "GameConnection": %b = %b.player;
			case "AIplayer": return true;
		}
		if(isObject(%a) && isObject(%b) && %a != %b)
		{
			if(%a.getClassName() $= "Player" && %b.getClassName() $= "Player")
			{
				%atribe = %a.client.account.read("Tribe");
				%btribe = %b.client.account.read("Tribe");
				return %atribe !$= %btribe;
			}
		}
		return %p;
	}
};
activatePackage("RFW_MinigameDamage");

if(isPackage("RFW_Combat"))
	deactivatePackage("RFW_Combat");

package RFW_Combat
{
	function ShapeBase::Damage(%this, %sourceObject, %position, %damage, %damageType, %wat)
	{
		if(%damage > 0)
		{
			//%this.addDamager(%sourceObject);
			if(%sourceObject.damageHandicap > 0)
				%damage *= %sourceObject.damageHandicap;

			if(%this.damageReduction > 0)
				%damage *= %this.damageReduction;

			if(!isEventPending(%this.HpRegenSched))
				%this.HpRegenSched = %this.schedule(12000, HpRegenSched);
		}
		
		%this.lastDamager = %sourceObject;
		%curExp = %this.exp;
		parent::Damage(%this, %sourceObject, %position, %damage, %damageType, %wat);
		if(%this.getState() $= "dead")
		{
			if(!%this.isDead && (isObject(%sourceClient = %sourceObject.sourceClient) || isObject(%sourceClient = %sourceObject.client)))
			{
				if(%curExp > 0 && isObject(%type = %this.type) && isObject(%acc = %sourceClient.account))
				{
					%sourceClient.addEXP(%curExp);
					%sourceClient.centerPrintQueue("\c3Exp\c2x" @ %curExp @ "/" @ %acc.vMaxExp, 3);
				}
			}

			%this.isDead = true;
		}
	}

	function gameConnection::onDeath(%this, %sObj, %sClient, %dmgType, %dmgArea)
	{
		%obj = %this.player;
		%obj.isDead = true;
		%obj.setButcherLoot(45, 20, 10);
		%msg = getDeathMessage(%dmgType);
		if(isObject(%sClient))
		{
			%msg = strReplace(%msg, "%1", %sClient.name);
		}
		else
		{
			%msg = strReplace(%msg, "%1", strReplace(%obj.lastDamager.type.name, "_", " "));
		}
		%msg = strReplace(%msg, "%2", %this.name);
		messageAll('', %msg);
		//prevent normal death message from showing up
		%dmgType = 0;
		%this.player.lastDirectDamageType = 0;
		parent::onDeath(%this, %sObj, %sClient, %dmgType, %dmgArea);
		%acc = %this.account;
		%i = 0;
		%check = 0;
		while(%i < 3 && %check++ < 100)
		{
			%r = getRandom(0, %acc.read("MaxItems") - 1);
			%itm = %acc.read("Item" @ %r);
			if(%itm !$= "")
			{
				%type = getItemType(%itm);
				if(!$RFW::ItemDrops[%type])
				{
					continue;
				}
				if($RFW::ItemStackable[%type])
				{
					%q = mFloor(getItemQuantity(%itm) * 0.2);
					if(%q == 0)
					{
						continue;
					}
					%itm = %type TAB %q TAB "";
				}
				%acc.removeItem(%itm, true);
				%obj.loot[%i] = %itm;
				%i++;
			}
		}
		//drop all crystal equipment
		%maxItems = %acc.read("MaxItems");
		for(%j = 0; %j < %maxItems; %j++)
		{
			%itm = %acc.read("Item" @ %j);
			if(firstWord(getItemType(%itm)) $= "Crystal")
			{
				%acc.removeItem(%itm, true);
				%obj.loot[%i] = %itm;
				%i++;
			}
		}
		%maxEquips = %acc.read("MaxEquips");
		for(%j = 0; %j < %maxEquips; %j++)
		{
			%itm = %acc.read("Equip" @ %j);
			if(firstWord(getItemType(%itm)) $= "Crystal")
			{
				%acc.set("Equip" @ %j, "");
				%obj.loot[%i] = %itm;
				%i++;
			}
		}
	}

	function Player::ActivateStuff(%this)
	{
		parent::ActivateStuff(%this);
		%scale = getWord(%this.getScale(), 2);
		%start = %this.getEyePoint();
		%targets = conalRaycastM(%start, %this.getEyeVector(), 5 * %scale, 30, $Typemasks::CorpseObjectType);
		%acc = %this.client.account;
		while(isObject(%hit = firstWord(%targets)))
		{
			%targets = restWords(%targets);

			%itm = %hit.loot[0];
			if(%itm !$= "")
			{
				%acc.addItem(%itm, true);
				for(%i = 1; %hit.loot[%i] !$= ""; %i++)
				{
					%hit.loot[%i - 1] = %hit.loot[%i];
				}
				%hit.loot[%i - 1] = "";
			}
		}
	}
};
activatePackage(RFW_Combat);