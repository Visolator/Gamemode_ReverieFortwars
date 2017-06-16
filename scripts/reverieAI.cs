//ReverieFortWars/reverieAI.cs

exec("./amdeAI.cs");

//TABLE OF CONTENTS
// #0. system
// #1. faerie
// #2. spooky skeleton
// #3. horse
// #4. exile
// #5. blood golem

$RFW::EnemyTypeExp[amdeAI_Faerie] = 40;
$RFW::EnemyTypeExp[amdeAI_Horse] = 15;
$RFW::EnemyTypeExp[amdeAI_ExileNewbie] = 4;
$RFW::EnemyTypeExp[amdeAI_ExileStone] = 6;
$RFW::EnemyTypeExp[amdeAI_ExileCalcite] = 8;
$RFW::EnemyTypeExp[amdeAI_ExileSangite] = 12;
$RFW::EnemyTypeExp[amdeAI_SkeletonWarrior] = 14;
$RFW::EnemyTypeExp[amdeAI_SkeletonRanger] = 18;
$RFW::EnemyTypeExp[amdeAI_ExileSanguite] = 20;
$RFW::EnemyTypeExp[amdeAI_Wendigo] = 32;
$RFW::EnemyTypeExp[amdeAI_BloodGolem] = 28;

$RFW::EnemyTypes["Grass"] = 3;
$RFW::EnemyType["Grass", 0] = 0.05 SPC amdeAI_Faerie;
$RFW::EnemyType["Grass", 1] = 0.2 SPC amdeAI_Horse;
$RFW::EnemyType["Grass", 2] = 0.4 SPC amdeAI_ExileNewbie;

$RFW::EnemyTypes["Sand"] = 3;
$RFW::EnemyType["Sand", 0] = 0.1 SPC amdeAI_SkeletonWarrior;
$RFW::EnemyType["Sand", 1] = 0.2 SPC amdeAI_SkeletonRanger;
$RFW::EnemyType["Sand", 2] = 0.25 SPC amdeAI_ExileNewbie;
$RFW::EnemyType["Sand", 3] = 0.35 SPC amdeAI_ExileStone;
$RFW::EnemyType["Sand", 4] = 0.4 SPC amdeAI_ExileCalcite;
$RFW::EnemyType["Sand", 5] = 0.45 SPC amdeAI_ExileSangite;

$RFW::EnemyTypes["Snow"] = 3;
$RFW::EnemyType["Snow", 0] = 0.1 SPC amdeAI_SkeletonWarrior;
$RFW::EnemyType["Snow", 1] = 0.2 SPC amdeAI_SkeletonRanger;
$RFW::EnemyType["Snow", 2] = 0.25 SPC amdeAI_ExileStone;
$RFW::EnemyType["Snow", 3] = 0.35 SPC amdeAI_ExileCalcite;
$RFW::EnemyType["Snow", 4] = 0.4 SPC amdeAI_ExileSangite;

$RFW::EnemyTypes["Stone"] = 3;
$RFW::EnemyType["Stone", 0] = 0.1 SPC amdeAI_SkeletonWarrior;
$RFW::EnemyType["Stone", 1] = 0.2 SPC amdeAI_SkeletonRanger;
$RFW::EnemyType["Stone", 2] = 0.25 SPC amdeAI_ExileNewbie;
$RFW::EnemyType["Stone", 3] = 0.35 SPC amdeAI_ExileStone;
$RFW::EnemyType["Stone", 4] = 0.45 SPC amdeAI_ExileCalcite;
$RFW::EnemyType["Stone", 5] = 0.5 SPC amdeAI_ExileSangite;

$RFW::EnemyTypes["Flesh"] = 4;
$RFW::EnemyType["Flesh", 0] = 0.1 SPC amdeAI_SkeletonWarrior;
$RFW::EnemyType["Flesh", 1] = 0.2 SPC amdeAI_SkeletonRanger;
$RFW::EnemyType["Flesh", 2] = 0.3 SPC amdeAI_ExileSanguite;
$RFW::EnemyType["Flesh", 3] = 0.9 SPC amdeAI_Wendigo;
$RFW::EnemyType["Flesh", 4] = 1.0 SPC amdeAI_BloodGolem;

// #0.

//this set keeps track of all active AIs
new simSet(RFW_AiGroup)
{
};

function AnyPlayerCanSee(%obj)
{
	%clientCount = clientGroup.getCount();
	for(%i = 0; %i < %clientCount; %i++)
	{
		if(isObject(%player = clientGroup.getObject(%i).player))
		{
			%hpos = %obj.getHackPosition();
			%eyevec = %player.getEyeVector();
			%eyept = %player.getEyePoint();
			%dvec = vectorNormalize(vectorSub(%hpos, %eyept));
			%dist = vectorDist(%hpos, %eyept);
			if(%dist < 40)
			{
				return true;
			}
			else if(%dist < 150 && getVectorAngle(%eyevec, %dvec) < 120)
			{
				%masks = $Typemasks::FxBrickObjectType; //don't really need other typemasks
				%ray = containerRaycast(%eyept, %hpos, %masks);
				if(!isObject(firstWord(%ray)))
				{
					return true;
				}
			}
		}
	}
	return false;
}

function AnyPlayerCanSeePos(%pos)
{
	%clientCount = clientGroup.getCount();
	for(%i = 0; %i < %clientCount; %i++)
	{
		%client = clientGroup.getObject(%i);
		if(isObject(%player = %client.player))
		{
			%eyevec = %player.getEyeVector();
			%eyept = %player.getEyePoint();
			%dvec = vectorNormalize(vectorSub(%pos, %eyept));
			%dist = vectorDist(%pos, %eyept);
			if(%dist < 150 && getVectorAngle(%eyevec, %dvec) < 120)
			{
				%masks = $Typemasks::FxBrickObjectType; //don't really need other typemasks
				%ray = containerRaycast(%eyept, %pos, %masks);
				if(!isObject(firstWord(%ray)))
				{
					return true;
				}
			}
		}
	}
	return false;
}

function PickAiSpawnPoint_AboveGround(%center)
{
	while(%crashCheck++ < 25)
	{
		%ang = $pi * (getRandom() - 0.5);
		%radius = getRandom(10, 35);
		%rpos = vectorAdd(%center, (%radius * mCos(%ang)) SPC (%radius * mSin(%ang)) SPC -50);
		%ray = containerRaycast(setWord(%rpos, 2, 500), %rpos, $Typemasks::FxBrickObjectType);
		if(isObject(%brick = firstWord(%ray)) && %brick.getGroup().bl_id == 888888)
		{
			%rpos = posFromRaycast(%ray);
			if(!AnyPlayerCanSeePos(%rpos))
			{
				return %brick TAB %rpos;
			}
		}
	}
	echo("\c2PickAiSpawnPoint_AboveGround crashcheck failed for center " @ %center);
	return 0 TAB %center;
}

function serverCmdSpawnMob(%this, %val)
{
	if(!%this.isSuperAdmin)
		return;

	if(!isObject(%player = %this.player))
		return;

	%ppos = %player.getPosition();
	%masks = $Typemasks::FxBrickObjectType;
	%ray = containerRaycast(%ppos, vectorAdd(%ppos, "0 0 20"), %masks);
	%underground = isObject(firstWord(%ray));
	%biome = getBiome(%ppos);
	if(%underground)
	{
	}
	else //above ground
	{
		%pos = vectorAdd(%ppos, getRandom(-6, 6) SPC getRandom(-6, 6) SPC 10);
		%r = getRandom();
		if(%val > 0)
			%r = %val;

		for(%j = 0; %j < $RFW::EnemyTypes[%biome]; %j++)
		{
			if(%r < firstWord($RFW::EnemyType[%biome, %j]))
			{
				%type = restWords($RFW::EnemyType[%biome, %j]);
				break;
			}
		}
		if(isObject(%type))
		{
			RFW_AiGroup.add(amdeAI_Create(%pos, %type));
		}
	}
}

//this function runs periodically to spawn AIs near players
function RFW_AiSpawn()
{
	cancel($RFW_AiSpawnSched);
	%clientCount = clientGroup.getCount();
	for(%i = 0; %i < %clientCount; %i++)
	{
		if(!isObject(%player = clientGroup.getObject(%i).player))
		{
			continue;
		}
		%ppos = %player.getPosition();
		%masks = $Typemasks::FxBrickObjectType;
		%ray = containerRaycast(%ppos, vectorAdd(%ppos, "0 0 20"), %masks);
		%underground = isObject(firstWord(%ray));
		%biome = getBiome(%ppos);
		if(%underground)
		{
		}
		else //above ground
		{
			%spawn = PickAiSpawnPoint_AboveGround(%ppos);
			%brick = getField(%spawn, 0);
			%spawn = getField(%spawn, 1);
			if(!isObject(%brick) || findClosestTotemDist(%spawn) != -1)
			{
				continue;
			}
			%r = getRandom();
			for(%j = 0; %j < $RFW::EnemyTypes[%biome]; %j++)
			{
				if(%r < firstWord($RFW::EnemyType[%biome, %j]))
				{
					%type = restWords($RFW::EnemyType[%biome, %j]);
					break;
				}
			}
			if(isObject(%type))
			{
				RFW_AiGroup.add(amdeAI_Create(%spawn, %type));
			}
		}
	}
	%interval = getRandom(70, 120) * 500;
	$RFW_AiSpawnSched = schedule(%interval, 0, RFW_AiSpawn);
}

$RFW_AiSpawnSched = schedule(90000, 0, RFW_AiSpawn);

//this function runs periodically to delete old AIs that are out of sight of and far away from any players
function RFW_AiCollectGarbage()
{
	cancel($RFW_AiCollectGarbageSched);
	%time = getSimTime();
	%count = RFW_AiGroup.getCount();
	for(%i = 0; %i < %count; %i++)
	{
		%obj = RFW_AiGroup.getObject(%i);
		if((%obj.lastSeenTime !$= "" ? %obj.lastSeenTime : %obj.creationTime) + %obj.type.garbageCollectionTimeout < %time && !isObject(%obj.target))
		{
			if(!AnyPlayerCanSee(%obj))
			{
				%obj.delete();
				%i--;
				%count--;
			}
			else
			{
				%obj.lastSeenTime = %time;
			}
		}
	}
	$RFW_AiCollectGarbageSched = schedule(10000, 0, RFW_AiCollectGarbage);
}

$RFW_AiCollectGarbageSched = schedule(180000, 0, RFW_AiCollectGarbage);

// #1.

new ScriptObject(amdeAI_Faerie)
{
	name = "Faerie"; //for function calls
	faerie = true;

	canMove = true;
	sightRange = 75;
	fov = 360;

	rangedWeapon = FaeFireImage;
	effectiveRange = 50;
	safeRange = 0;
	prefersRanged = true;
	rangedOnly = false;

	garbageCollectionTimeout = 60000;

	meleeWeapon = FaeFireImage;
};

function amdeAI_Faerie_OnAdd(%bot)
{
	%bot.setDatablock(PlayerFaerie);
	%bot.mountImage(FaerieImage, 1);
	%bot.damageReduction = 0.5;
	%bot.creationTime = getSimTime();
	//the faerie image will hide all nodes for us
}

function amdeAI_Faerie_ShouldAttack(%bot, %target)
{
	if(%target.getClassName() $= "AiPlayer" && %target.type.name !$= "Faerie" && %target.type.name !$= "Horse" && %target.type.name !$= "Exile")
	{
		return true;
	}
	return %bot.hates[%target];
}

function amdeAI_Faerie_OnDetectAlly(%bot, %ally)
{
	if(%bot.lastHelloTime + 30000 < getSimTime() && %ally.getClassName() $= "Player" && vectorDist(%bot.getPosition(), %ally.getPosition()) < 15)
	{
		ServerPlay3d(FaerieHello, %bot.getEyePoint());
		%bot.lastHelloTime = getSimTime();
	}
}

function amdeAI_Faerie_Attack(%bot, %target)
{
	%bot.setImageTrigger(0, 1);
	%bot.setImageTrigger(0, 0);
}

function amdeAI_Faerie_OnDamage(%bot, %sourceObject, %position, %damage, %damageType)
{
	if(!isObject(%bot.target) && isObject(%sourceObject))
	{
		switch$(%sourceObject.getClassName())
		{
			case "Player":
				%bot.target = %sourceObject;
			case "Projectile":
				%bot.target = %sourceObject.sourceObject;
			default:
				%bot.target = "";
		}
		%bot.hates[%bot.target] = true;
		%bot.amdeAI_Sched();
	}
	//aggro all nearby faeries
	%time = getSimTime();
	%eyePos = %bot.getEyePoint();
	if(%bot.lastScreamTime + 30000 < %time)
	{
		%bot.lastScreamTime = %time;
		ServerPlay3d(FaerieWatchout, %eyePos);
	}
	InitContainerRadiusSearch(%eyePos, %bot.type.sightrange, $Typemasks::PlayerObjectType);
	while(%hit = ContainerSearchNext())
	{
		if(%hit.getClassName() $= "AiPlayer" && %hit.type.faerie && !isObject(%hit.target))
		{
			%hit.target = %bot.target;
			%hit.amdeAI_Sched();
			%hit.hates[%bot.target] = true;
		}
	}
	//play loud scream?
}

function amdeAI_Faerie_OnDeath(%bot)
{
	%bot.unbutcherable = true;
	%lootcount = -1;
	%bot.loot[%lootcount++] = "Faerie dust" TAB getRandom(6, 8);
	if(getRandom() > 0.5)
	{
		%bot.loot[%lootcount++] = "Plant matter" TAB getRandom(2, 18);
	}
	if(getRandom() > 0.7)
	{
		%bot.loot[%lootcount++] = "Wood\t1";
	}
}

// #2.
new ScriptObject(amdeAI_SkeletonRanger)
{
	name = "Skeleton";

	canMove = true;
	sightRange = 50;
	fov = 135;

	rangedWeapon = BoneBowImage;
	effectiveRange = 30;
	safeRange = 10;
	prefersRanged = true;
	rangedOnly = false;

	garbageCollectionTimeout = 30000;

	meleeWeapon = BoneSwordImage;
};

new ScriptObject(amdeAI_SkeletonWarrior)
{
	name = "Skeleton";

	canMove = true;
	sightRange = 50;
	fov = 135;

	rangedWeapon = "";
	effectiveRange = 0;
	safeRange = 0;
	prefersRanged = false;
	rangedOnly = false;

	garbageCollectionTimeout = 30000;

	meleeWeapon = BoneSwordImage;
};

function amdeAI_Skeleton_OnAdd(%bot)
{
	%bot.setDatablock(ReveriePlayer);
	%bot.damageReduction = 0.75;
	%bot.damageHandicap = 0.5;
	%bot.creationTime = getSimTime();
	%bot.setNodeColor("ALL", "1 1 1 1");
	%bot.hideNode("chest");
	%bot.hideNode("lArm");
	%bot.hideNode("rArm");
	%bot.unhideNode("lArmSlim");
	%bot.unhideNode("rArmSlim");
	%bot.mountImage(RibcageImage, 2);
	if(getRandom() > 0.8)
	{
		%bot.hideNode("headSkin");
	}
	if(getRandom() > 0.8)
	{
		%bot.damageReduction = 0.6;
		%bot.unhideNode("armor");
		%bot.unhideNode("shoulderPads");
		%bot.setNodeColor("armor", "0.3 0.3 0.25 1");
		%bot.setNodeColor("shoulderPads", "0.3 0.3 0.25 1");
	}
	%bot.setFaceName("Orc");
}

function amdeAI_Skeleton_ShouldAttack(%bot, %target)
{
	if(%target.getClassName() $= "Player" || %target.type.name $= "Faerie")
	{
		return true;
	}
}

function amdeAI_Skeleton_Attack(%bot, %target)
{
	%bot.setImageTrigger(0, 1);
	%bot.setImageTrigger(0, 0);
}

function amdeAI_Skeleton_OnDamage(%bot, %sourceObject, %position, %damage, %damageType)
{
	if(!isObject(%bot.target) && isObject(%sourceObject))
	{
		switch$(%sourceObject.getClassName())
		{
			case "Player":
				%bot.target = %sourceObject;
			case "Projectile":
				%bot.target = %sourceObject.sourceObject;
			default:
				%bot.target = "";
		}
		%bot.amdeAI_Sched();
	}
}

function amdeAI_Skeleton_OnDeath(%bot, %pos)
{
	%bot.setButcherLoot(0, 0, 10);
	%lootcount = -1;
	if(getRandom() > 0.9)
	{
		%bot.loot[%lootcount++] = "Bone bow\t\t";
		%bot.loot[%lootcount++] = "Bone arrow" TAB getRandom(5, 15);
	}
	if(getRandom() > 0.9)
	{
		%bot.loot[%lootcount++] = "Bone sword\t\t";
	}
}

function amdeAI_Skeleton_OnDetectEnemy(%bot, %target)
{
	%time = getSimTime();
	if(%bot.lastGrowlTime + 45000 < %time)
	{
		%bot.lastGrowlTime = %time;
		ServerPlay3d(MonsterGrowl, %bot.getEyePoint());
	}
}

// #3.
new ScriptObject(amdeAI_Horse)
{
	name = "Horse";

	canMove = true;
	sightRange = 30;
	fov = 90;

	rangedWeapon = "";
	effectiveRange = 30;
	safeRange = 99;
	prefersRanged = true;
	rangedOnly = true;

	garbageCollectionTimeout = 60000;

	meleeWeapon = "";
};

function amdeAI_Horse_onAdd(%bot)
{
	%bot.setDatablock(ReverieHorse);
	%bot.mountVehicle = true;
	switch(getRandom(1, 6))
	{
		case 1: %bot.setNodeColor("ALL", "0.8 0.8 0.8 1");
		case 2: %bot.setNodeColor("ALL", "0.4 0.4 0.4 1");
		case 3: %bot.setNodeColor("ALL", "0.4 0.3 0.2 1");
		case 4: %bot.setNodeColor("ALL", "0.9 0.8 0.3 1");
		case 5: %bot.setNodeColor("ALL", "0.1 0.1 0.1 1");
		case 6: %bot.setNodeColor("ALL", "0.8 0.3 0.2 1");
	}
}

function amdeAI_Horse_ShouldAttack(%bot, %target)
{
	//horse will always flee, so this function is more like ShouldFlee than ShouldAttack
	if(%target.getClassName() $= "AiPlayer" && %target.type.name $= "Skeleton")
	{
		return true;
	}
	return %bot.hates[%target];
}

function amdeAI_Horse_OnDamage(%bot, %sourceObject, %position, %damage, %damageType)
{
	if(isObject(%sourceObject))
	{
		switch$(%sourceObject.getClassName())
		{
			case "Player":
				%bot.target = %sourceObject;
			case "Projectile":
				%bot.target = %sourceObject.sourceObject;
			default:
				%bot.target = "";
		}
		%bot.amdeAI_Sched();
	}
}

function amdeAI_Horse_Retreat(%bot, %target)
{
	%time = getSimTime();
	if(%bot.lastNeighTime + 45000 < %time)
	{
		%bot.lastNeighTime = %time;
		ServerPlay3d(HorseNeigh, %bot.getEyePoint());
	}
	//default retreat functionality
	%bot.setMoveObject(0);
	%bot.clearAim();
	%run = vectorNormalize(vectorSub(setWord(%tlpos, 2, 0), setWord(%target.getPosition(), 2, 0)));
	%run = vectorAdd(%tlpos, vectorScale(%run, 50));
	%bot.setMoveDestination(%run);
	%bot.setJumping(true);
	%bot.schedule(100, setJumping, false);
	return true;
}

function amdeAI_Horse_OnDeath(%bot, %pos)
{
	%bot.setButcherLoot(90, 40, 20);
}

// #4.

mutalidZombie.jumpSound = mutalidJumpSound;
mutalidZombie.isInvincible = false;

function ZombieDefault::onCollision() { }
function ZombieDefault::onDisabled() { }

function MclawProjectile::onCollision(%this, %obj, %col, %fade, %pos, %normal)
{
	if(%col.getType() & $Typemasks::PlayerObjectType && minigameCanDamage(%obj.sourceObject, %col))
	{
		%col.damage(%obj.sourceObject, %pos, 35, $DamageType::Mclaw);
	}
}

new ScriptObject(amdeAI_Wendigo)
{
	name = "Wendigo";

	canMove = true;
	sightRange = 50;
	fov = 135;

	rangedWeapon = "";
	effectiveRange = 0;
	safeRange = 0;
	prefersRanged = false;
	rangedOnly = false;

	garbageCollectionTimeout = 50000;

	meleeWeapon = mclawimage;
};

function amdeAI_Wendigo_OnAdd(%bot)
{
	%bot.setDatablock(mutalidZombie);
	%bot.damageReduction = 1;
	%bot.creationTime = getSimTime();
	%bot.setNodeColor("ALL", 0.5 + getRandom() * 0.4 SPC getRandom() * 0.2 SPC getRandom() * 0.2 SPC 1);
}

function amdeAI_Wendigo_ShouldAttack(%bot, %target)
{
	if(%target.getClassName() $= "Player" || %target.type.name $= "Faerie" || %target.type.name $= "Horse" || %target.type.name $= "Exile")
	{
		return true;
	}
}

function amdeAI_Wendigo_Attack(%bot, %target)
{
	%bot.setImageTrigger(0, 1);
	%bot.setImageTrigger(0, 0);
}

function amdeAI_Wendigo_OnDamage(%bot, %sourceObject, %position, %damage, %damageType)
{
	if(!isObject(%bot.target) && isObject(%sourceObject))
	{
		switch$(%sourceObject.getClassName())
		{
			case "Player":
				%bot.target = %sourceObject;
			case "Projectile":
				%bot.target = %sourceObject.sourceObject;
			default:
				%bot.target = "";
		}
		%bot.amdeAI_Sched();
	}
}

function amdeAI_Wendigo_OnDeath(%bot, %pos)
{
	%bot.butcherLoot = "15 Bone\t20 Blood\t70 Flesh\t15 Calcite\t10 Sanguite";
}

function amdeAI_Wendigo_OnDetectEnemy(%bot, %target)
{
	%time = getSimTime();
	if(%bot.lastGrowlTime + 45000 < %time)
	{
		%bot.lastGrowlTime = %time;
		ServerPlay3d(MonsterGrowl, %bot.getEyePoint());
	}
}

// #4.
new ScriptObject(amdeAI_ExileNewbie)
{
	name = "Exile";
	tier = 0;

	canMove = true;
	sightRange = 50;
	fov = 135;

	rangedWeapon = "";
	effectiveRange = 0;
	safeRange = 0;
	prefersRanged = false;
	rangedOnly = false;

	garbageCollectionTimeout = 30000;

	meleeWeapon = fistsImage;
};

new ScriptObject(amdeAI_ExileStone)
{
	name = "Exile";
	tier = 1;

	canMove = true;
	sightRange = 50;
	fov = 135;

	rangedWeapon = "";
	effectiveRange = 0;
	safeRange = 0;
	prefersRanged = false;
	rangedOnly = false;

	garbageCollectionTimeout = 30000;

	meleeWeapon = stoneSwordImage;
};

new ScriptObject(amdeAI_ExileCalcite)
{
	name = "Exile";
	tier = 4;

	canMove = true;
	sightRange = 50;
	fov = 135;

	rangedWeapon = "";
	effectiveRange = 0;
	safeRange = 0;
	prefersRanged = false;
	rangedOnly = false;

	garbageCollectionTimeout = 30000;

	meleeWeapon = calciteSwordImage;
};

new ScriptObject(amdeAI_ExileSanguite)
{
	name = "Exile";
	tier = 6;

	canMove = true;
	sightRange = 50;
	fov = 135;

	rangedWeapon = "";
	effectiveRange = 0;
	safeRange = 0;
	prefersRanged = false;
	rangedOnly = false;

	garbageCollectionTimeout = 30000;

	meleeWeapon = sanguiteSwordImage;
};

function amdeAI_Exile_OnAdd(%bot)
{
	%bot.setDatablock(ReveriePlayer);
	%bot.creationTime = getSimTime();
	%skin = $RFW::Skin[getRandom(0, $RFW::SkinColors - 1)];
	%tier = %bot.type.tier;
	%bot.aggressive = getRandom() < %tier / 10;
	if(%tier == 0)
	{
		%bot.setNodeColor("ALL", %skin);
		%cloth = %skin $= "0.4 0.2 0 1" ? $RFW::Pants[1] : $RFW::Pants[0];
		%bot.setNodeColor("pants", %cloth);
	}
	else
	{
		%cloth = vectorScale(getRandom() SPC getRandom() SPC getRandom(), 0.5) SPC 1;
		switch(%tier)
		{
			case 1: %metal = getColorIdTable($RFW::ResourceColor["Stone"]);
			case 2: %metal = getColorIdTable($RFW::ResourceColor["Obsidian"]);
			case 3: %metal = getColorIdTable($RFW::ResourceColor["Nithlite"]);
			case 4: %metal = getColorIdTable($RFW::ResourceColor["Calcite"]);
			case 5: %metal = getColorIdTable($RFW::ResourceColor["Sinnite"]);
			case 6: %metal = getColorIdTable($RFW::ResourceColor["Sanguite"]);
			case 7: %metal = getColorIdTable($RFW::ResourceColor["Crystal"]);
		}
		%bot.setNodeColor("chest", %cloth);
		%bot.setNodeColor("pants", setWord(%cloth, 2, 0));
		%bot.unhideNode("armor");
		%bot.setNodeColor("armor", %metal);
		%bot.unhideNode("shoulderPads");
		%bot.setNodeColor("shoulderPads", %metal);
		%bot.setNodeColor("Lhand", setWord(%metal, 3, 1));
		%bot.setNodeColor("Rhand", setWord(%metal, 3, 1));
		%bot.setNodeColor("Lshoe", setWord(%metal, 3, 1));
		%bot.setNodeColor("Rshoe", setWord(%metal, 3, 1));
		if(getRandom() < 0.5)
		{
			%hat = $Hat[getRandom(1, 3)];
			%bot.unhideNode(%hat);
			%bot.setNodeColor(%hat, %metal);
		}
		else if(getRandom() < 0.3)
		{
			%hat = $Hat[getRandom(4, 7)];
			%bot.unhideNode(%hat);
			%bot.setNodeColor(%hat, %cloth);
		}
	}
	switch(getRandom(0, 10))
	{
		case 0:
			%bot.setFacename("smileyBlonde");
		case 1:
			%bot.setFacename("smileyCreepy");
		case 2:
			%bot.setFacename("smileyEvil1");
		case 3:
			%bot.setFacename("smileyEvil2");
		case 4:
			%bot.setFacename("smileyFemale1");
			%bot.setNodeColor("chest", %cloth);
			%bot.setNodeColor("femChest", %cloth);
		case 5:
			%bot.setFacename("BrownSmiley");
		case 6:
			%bot.setFacename("Male07Smiley");
		default:
			%bot.setFacename("smiley");
	}
}

function amdeAI_Exile_ShouldAttack(%bot, %target)
{
	return %bot.aggressive || %bot.hates[%target];
}

function amdeAI_Exile_OnDamage(%bot, %sourceObject, %position, %damage, %damageType)
{
	if(!isObject(%bot.target) && isObject(%sourceObject))
	{
		switch$(%sourceObject.getClassName())
		{
			case "Player":
				%bot.target = %sourceObject;
			case "Projectile":
				%bot.target = %sourceObject.sourceObject;
			default:
				%bot.target = "";
		}
		%bot.hates[%bot.target] = true;
		%bot.amdeAI_Sched();
	}
}

function amdeAI_Exile_OnDeath(%bot, %pos)
{
	%bot.setButcherLoot(45, 20, 10);
	%lootcount = -1;
	%tier = %this.type.tier;
	if(%tier == 0) //newbie
	{
		%bot.loot[%lootcount++] = "Wood" TAB getRandom(5, 20);
		%bot.loot[%lootcount++] = "Stone" TAB getRandom(5, 20);
		if(getRandom() < 0.5)
		{
			%bot.loot[%lootcount++] = "Plant matter" TAB getRandom(5, 20);
		}
		else
		{
			%bot.loot[%lootcount++] = "Dirt" TAB getRandom(5, 20);
		}
	}
	else if(%tier == 1) //stone
	{
		%bot.loot[%lootcount++] = "Stone" TAB getRandom(15, 30);
		%bot.loot[%lootcount++] = "Cloth" TAB getRandom(5, 15);
		if(getRandom() < 0.5)
		{
			%bot.loot[%lootcount++] = "Surtra" TAB getRandom(5, 15);
		}
	}
	else if(%tier == 4)  //calcite
	{
		%bot.loot[%lootcount++] = "Cloth" TAB getRandom(5, 15);
		%bot.loot[%lootcount++] = "Calcite" TAB getRandom(4, 12);
		if(getRandom() < 0.04)
		{
			switch(getRandom(1, 5))
			{
				case 1: %bot.loot[%lootcount++] = "Calcite sword\t0\t";
				case 2: %bot.loot[%lootcount++] = "Calcite dagger\t0\t";
				case 3: %bot.loot[%lootcount++] = "Calcite armor\t\t";
				case 4: %bot.loot[%lootcount++] = "Calcite quiver\t\t";
				case 5: %bot.loot[%lootcount++] = "Sanguite" TAB getRandom(3, 9);
			}
		}
	}
	else if(%tier == 6) //sanguite
	{
		%bot.loot[%lootcount++] = "Cloth" TAB getRandom(5, 15);
		%bot.loot[%lootcount++] = "Sanguite" TAB getRandom(4, 12);
		if(getRandom() < 0.03)
		{
			switch(getRandom(1, 4))
			{
				case 1: %bot.loot[%lootcount++] = "Sanguite sword\t0\t";
				case 2: %bot.loot[%lootcount++] = "Sanguite dagger\t0\t";
				case 3: %bot.loot[%lootcount++] = "Sanguite armor\t\t";
				case 4: %bot.loot[%lootcount++] = "Sanguite quiver\t\t";
			}
		}
	}
}

// #5.
new ScriptObject(amdeAI_BloodGolem)
{
	name = "Blood_golem";

	canMove = true;
	sightRange = 30;
	fov = 110;

	rangedWeapon = "";
	effectiveRange = 0;
	safeRange = 0;
	prefersRanged = false;
	rangedOnly = false;

	garbageCollectionTimeout = 120000;

	meleeWeapon = mClawImage;
};

function amdeAI_Blood_golem_OnAdd(%bot)
{
	%scale = getRandom(25, 35) / 10;
	%bot.setDatablock(ReveriePlayer);
	%bot.damageReduction = 0.5;
	%bot.damageHandicap = 0.5;
	%bot.creationTime = getSimTime();
	%bot.setPlayerScale(%scale);
	%bot.setMaxForwardSpeed(8 / %scale);
	%bot.setMaxSideSpeed(6 / %scale);
	%bot.setMaxBackwardSpeed(4 / %scale);
	%bot.setNodeColor("ALL", (getRandom(3, 8) / 10) SPC "0 0 1");
}

function amdeAI_Blood_golem_ShouldAttack(%bot, %target)
{
	if(%target.getClassName() $= "Player" || %target.type.name $= "Faerie" || %target.type.name $= "Horse" || %target.type.name $= "Exile")
	{
		return true;
	}
}

function amdeAI_Blood_golem_Pathfind(%bot, %target)
{
	return;
}

function amdeAI_Blood_golem_OnDeath(%bot, %pos)
{
	%scale = getWord(%bot.getScale(), 2);
	%bot.sanguification = mClampF(%bot.sanguification, 0.5, 1);
	%bot.setButcherLoot(20 * %scale, 50 * %scale, 0);
}