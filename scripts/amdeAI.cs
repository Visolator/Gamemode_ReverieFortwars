///////////////
// amdeAI.cs //
///////////////
// reverent version

// table of contents

// #1. basic AI functionality
//   #1.1 amdeAI_call
//   #1.2 amdeAI_Sched (main functionality)
//   #1.3 amdeAI_create
//   #1.4 package
// #2. default AI type

// #1.

// #1.1
function amdeAI_call(%fn, %bot, %a1, %a2, %a3, %a4, %a5)
{
	return call(isFunction("amdeAI_" @ %bot.type.name @ "_" @ %fn) ? "amdeAI_" @ %bot.type.name @ "_" @ %fn : "amdeAI_Default_" @ %fn, %bot, %a1, %a2, %a3, %a4, %a5);
}

// #1.2
function AIPlayer::amdeAI_Sched(%this)
{
	if(!isObject(%this))
	{
		return;
	}
	if(%this.isDead)
	{
		%this.stop();
		return;
	}
	cancel(%this.jumpsched);
	cancel(%this.amdeAI_Sched);
	%this.dismount();
	%schedlen = getRandom(1000, 1500);
	%this.amdeAI_Sched = %this.schedule(%schedlen, amdeAI_Sched);
	%tepos = %this.getEyePoint();
	%tlpos = %this.getPosition();
	%type = %this.type;
	%hasTarget = (isObject(%target = %this.target) && %target.getState() !$= "Dead" && vectorDist(%tepos, %targpos = %target.getPosition()) <= %type.sightrange * 2);
	if(%hasTarget)
	{
		if(amdeAI_call("CanSee", %this, %target))
		{
			//weapon selection
			%dist = vectorDist(%tlpos, %target.getPosition());
			if(%dist > 5)
			{
				//ranged
				if(isObject(%image = nameToID(%type.rangedWeapon)) && %this.getMountedImage(0) != %image)
				{
					%this.updateArm(%image);
					%this.mountImage(%image, 0);
				}
			}
			else if(!%type.rangedOnly)
			{
				//melee
				if(isObject(%image = nameToID(%type.meleeWeapon)) && %this.getMountedImage(0) != %image)
				{
					%this.updateArm(%image);
					%this.mountImage(%image, 0);
				}
				//strafing
				%this.setMoveX(getRandom(-1, 1));
				%this.schedule(%schedlen / 2, clearMoveX);
			}
			if(%type.canMove)
			{
				//chase behavior
				if(%type.rangedOnly || (%dist > 5 && %type.prefersRanged))
				{
					//ranged chase behavior
					if(%dist > %type.effectiveRange + 5)
					{
						//too far away: advance
						%this.setMoveObject(%target);
						switch$(amdeAI_call("Pathfind", %this, %target))
						{
							case "Jump": %this.setJumping(true); %this.schedule(%schedlen / 2, setJumping, false);
							case "Crouch": %this.setCrouching(true); %this.schedule(%schedlen / 2, setCrouching, false);
						}
						//attack
						%this.setAimObject(%target);
					}
					else if(%dist < %type.safeRange)
					{
						//too close: retreat
						if(amdeAI_call("Retreat", %this, %target))
						{
							return;
						}
					}
					else
					{
						//sweet spot: backpedal
						%this.setMoveObject(0);
						%this.stop();
						if(%dist + 5 < %this.effectiveRange)
						{
							%this.setMoveY(-1);
							%this.schedule(%schedlen - 1, clearMoveY);
						}
						//attack
						%this.setAimObject(%target);
					}
				}
				else
				{
					//melee chase behavior
					%this.setMoveObject(%target);
					%this.setAimObject(%target);
					switch$(amdeAI_call("Pathfind", %this, %target))
					{
						case "Jump": %this.setJumping(true); %this.schedule(%schedlen / 2, setJumping, false);
						case "Crouch": %this.setCrouching(true); %this.schedule(%schedlen / 2, setCrouching, false);
					}
				}
			}
			//attack
			amdeAI_call("Attack", %this, %target);
			//store target info
			%this.lastSeenTarget = getSimTime();
			%this.lastSeenTargetPos = %targpos;
			%noWander = true;
		}
		else
		{
			//can't see target
			%this.setImageTrigger(0, 0);
			%this.setImageTrigger(1, 0);
			%this.setMoveObject(0);
			%this.clearAim();
			if(%this.lastSeenTarget + 5000 < getSimTime())
			{
				//lost target
				%hasTarget = false;
				if(isObject(%this.target))
				{
					amdeAI_call("OnLoseTarget", %this, %target);
				}
			}
			else if(%this.canMove)
			{
				%this.setMoveDestination(%this.lastSeenTargetPos);
				%noWander = true;
			}
		}
	}
	if(!%hasTarget)
	{
		//clear attacks
		%this.setImageTrigger(0, 0);
		%this.setImageTrigger(1, 0);
		//clear target info
		%this.lastSeenTarget = 0;
		%this.lastSeenTargetPos = 0;
		%this.target = 0;
		//clear movement
		%this.setMoveObject(0);
		%this.clearMoveX();
		%this.clearAim();
	}
	//this search runs even if it has a target, just in case there's a better target around
	InitContainerRadiusSearch(%tepos, %type.sightrange, $Typemasks::PlayerObjectType);
	while(%hit = ContainerSearchNext())
	{
		if(amdeAI_call("CanSee", %this, %hit) && %hit.getState() !$= "Dead")
		{
			%hpos = %hit.getHackPosition();
			%eyevec = %this.getEyeVector();
			%dvec = vectorNormalize(vectorSub(%hpos, %tepos));
			%nFOV = getVectorAngle(%eyevec, %dvec);
			if(%nFOV <= %type.fov)
			{
				if(amdeAI_call("ShouldAttack", %this, %hit))
				{
					amdeAI_call("OnDetectEnemy", %this, %hit);
					if(!%haveenemy)
					{
						%this.target = %hit;
						if(%this.canMove && (vectorDist(%hpos, %tepos) > 5 && isObject(%this.item[0])))
						{
							%this.setMoveObject(%hit);
						}
						%this.setAimObject(%hit);
						amdeAI_call("OnChase", %this, %hit);
						%haveenemy = true;
					}
				}
				else
				{
					amdeAI_call("OnDetectAlly", %this, %hit);
				}
			}
		}
	}
	if(!%noWander)
	{
		%dest = vectorAdd(%tepos, (getRandom() - 0.5) * 50 SPC (getRandom() - 0.5) * 50 SPC 0);
		while(isObject(firstWord(containerRaycast(%tlpos, %dest, $TypeMasks::FxBrickAlwaysObjectType | $TypeMasks::InteriorObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::VehicleBlockerObjectType, %this))))
		{
			%dest = vectorAdd(%tepos, (getRandom() - 0.5) * 20 SPC (getRandom() - 0.5) * 20 SPC 0);
			if(%i++ > 10) //This is here so if the object is encased in bricks or something the server doesn't crash
			{
				break;
			}
		}
		if(%type.canMove)
		{
			%this.setMoveDestination(%dest);
		}
		else
		{
			%this.setAimLocation(%dest);
		}
	}
}

// #1.3
function amdeAI_Create(%pos, %type)
{
	%bot = new AIplayer()
	{
		position = %pos;
		datablock = playerStandardArmor;
		type = %type;
		exp = $RFW::EnemyTypeExp[%type.getName()];
	};
	%bot.setMoveSlowdown(false);
	%bot.setMoveTolerance(3);
	amdeAI_call("onAdd", %bot);
	%bot.schedule(1, amdeAI_sched);
	%bot.spawnTime = getSimTime();
	%bot.setShapeNameDistance(60);
	return %bot;
}

// #1.4
package amde_AI
{
	// #1.4.1
	function AIplayer::Damage(%this, %sourceObject, %position, %damage, %damageType)
	{
		//%this.addDamager(%sourceObject);
		parent::Damage(%this, %sourceObject, %position, %damage, %damageType);
		if(%this.getState() $= "dead")
		{
			//it ded
			amdeAI_call("OnDeath", %this, %this.getHackPosition());
		}
		else
		{
			amdeAI_call("OnDamage", %this, %sourceObject, %position, %damage, %damageType);
		}
	}
};
activatePackage(amde_AI);

// #2.
new ScriptObject(amdeAI_Default)
{
	name = "Default"; //for function calls

	canMove = true;
	sightRange = 80;
	fov = 180;

	rangedWeapon = woodenBowImage;
	effectiveRange = 35;
	safeRange = 10;
	prefersRanged = false; //advance even if already in effective range?
	rangedOnly = false; //retreat instead of engage in melee combat?

	meleeWeapon = woodenSwordImage;
};

function amdeAI_Default_OnAdd(%bot)
{
	//appearance stuff
	%bot.hideNode("rshoe");
	%bot.hideNode("lshoe");
	%bot.hideNode("chest");
	%bot.hideNode("larm");
	%bot.hideNode("rarm");
	%bot.unhideNode("rpeg");
	%bot.unhideNode("lpeg");
	%bot.unhideNode("femChest");
	%bot.unhideNode("larmslim");
	%bot.unhideNode("rarmslim");
	%bot.setNodeColor("ALL", "1 1 1 1");
}

function amdeAI_Default_Pathfind(%bot, %target)
{
	%oh = %bot.getEyePoint();
	%th = %target.getEyePoint();
	%tl = vectorAdd(%target.getPosition(), "0 0 0.1");
	%typemasks = $Typemasks::InteriorObjectType | $Typemasks::TerrainObjectType;
	%types = $Typemasks::PlayerObjectType | $Typemasks::FXbrickAlwaysObjectType;
	%headRaycast = containerRaycast(%oh, %th, %typemasks | %types, %bot);
	%hit = firstWord(%headRaycast);
	%headCanSee = %hit == %target;
	%legRaycast = containerRaycast(%oh, %tl, %typemasks | %types, %bot);
	%hit = firstWord(%legRaycast);
	%legsCanSee = %hit == %target;
	if(%legsCanSee && !%headCanSee)
	{
		return "Crouch";
	}
	if(%headCanSee && !%legsCanSee)
	{
		return "Jump";
	}
	%oz = getWord(%bot.getPosition(), 2) + 0.1;
	%tz = getWord(%tl, 2);
	%ow = %bot.getWaterCoverage() > 0.5;
	%tw = %target.getWaterCoverage() > 0.5;
	if(%ow && %tw)
	{
		if(%oz < %tz)
		{
			return "Crouch";
		}
		if(%tz < %oz)
		{
			return "Jump";
		}
		return "None";
	}
	if(%tz > %oz + 0.6)
	{
		return "Jump";
	}
	return "None";
}

function amdeAI_Default_ShouldAttack(%bot, %target)
{
	return minigameCanDamage(%bot, %target);
}

function amdeAI_Default_Attack(%bot, %target)
{
	%bot.setImageTrigger(0, 1);
	%bot.schedule(%schedlen * 0.3, setImageTrigger, 1, 1);
	%bot.schedule(%schedlen * 0.7, setImageTrigger, 0, 0);
	%bot.schedule(%schedlen - 1, setImageTrigger, 1, 0);
}

function amdeAI_Default_CanSee(%bot, %target)
{
	%opos = %bot.getEyePoint();
	%th = %target.getEyePoint();
	%tl = %target.getPosition();
	%typemasks = $Typemasks::InteriorObjectType | $Typemasks::TerrainObjectType;
	%types = $Typemasks::PlayerObjectType | $Typemasks::FXbrickAlwaysObjectType;
	%headRaycast = containerRaycast(%opos, %th, %typemasks | %types, %bot);
	%hit = firstWord(%headRaycast);
	if(isObject(%hit) && (%type = %hit.getType()) & %types)
	{
		if(%hit == %target)
		{
			return true;
		}
		else if(%type & $Typemasks::PlayerObjectType)
		{
			return false;
		}
		else //Has to be a brick
		{
			if(!%hit.isRendering() || getWord(getColorIDtable(%hit.colorID), 3) < 1)
			{
				return true;
			}
		}
	}
	%legRaycast = containerRaycast(%opos, %tl, %typemasks, %bot);
	%hit = firstWord(%legRaycast);
	if(isObject(%hit) && (%type = %hit.getType()) & %types)
	{
		if(%hit == %target)
		{
			return true;
		}
		else if(%type & $Typemasks::PlayerObjectType)
		{
			return false;
		}
		else
		{
			if(!%hit.isRendering() || getWord(getColorIDtable(%hit.colorID), 3) < 1)
			{
				return true;
			}
		}
	}
	return false;
}

function amdeAI_Default_OnDetectEnemy(%bot, %target)
{
}

function amdeAI_Default_onChase(%bot, %target)
{
}

function amdeAI_Default_Retreat(%bot, %target)
{
	%bot.setMoveObject(0);
	%bot.clearAim();
	%run = vectorNormalize(vectorSub(setWord(%tlpos, 2, 0), setWord(%target.getPosition(), 2, 0)));
	%run = vectorAdd(%tlpos, vectorScale(%run, 50));
	%bot.setMoveDestination(%run);
	return true;
}

function amdeAI_Default_OnDamage(%bot, %sourceObject, %position, %damage, %damageType)
{
}

function amdeAI_Default_OnDeath(%bot, %pos)
{
}

function amdeAI_Default_OnLoseTarget(%bot, %target)
{
}

function amdeAI_Default_OnDetectAlly(%bot, %ally)
{
}