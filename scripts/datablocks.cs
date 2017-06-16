//ReverieFortWars/datablocks.cs

//TABLE OF CONTENTS
// #1. misc stuff
//   #1.1 player datablock
//   #1.2 generic item
//   #1.3 16x16 brick
// #2. Melee weapons
//   #2.1 general functionality
//   #2.2 Fists
//   #2.3 Swords
//   #2.4 pickaxes
//   #2.5 daggers
// #3. ranged weapons
//   #3.1 bows
//   #3.2 snowball
// #4. magic weapons
//   #4.1 faerie fire
//   #4.2 faerie wand
//   #4.3 photometallisynthesis lens
//   #4.4 pyromancy wand
//   #4.5 ossimetallisynthesis staff
//   #4.6 sanguimetallisynthesis wand
// #5. enemies
//   #5.1 faerie
//   #5.2 skeleton
//   #5.3 horse

// #1.
// #1.1
datablock PlayerData(ReveriePlayer : PlayerStandardArmor)
{
	canJet = false;
	isInvincible = false;

	showEnergyBar = true;
	rechargeRate = 0.01;
	runEnergyDrain = 0.01;

	maxForwardSpeed = 7 * 1.5;
	maxSideSpeed = 6 * 1.5;
	maxBackwardSpeed = 4 * 1.5;

	runForce = 4320;
	jumpForce = 1080;

	maxStepHeight = 1.25;

	jumpSound = ""; //see footsteps.cs

	uiName = "Reverie Player";
};

// #1.2

datablock ItemData(genericItem)
{
	category = "Weapon";
	className = "Weapon";

	shapeFile = "Add-Ons/GameMode_Blockheads_Ruin_Xmas/present.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	uiName = "generic item";
	iconName = "";
	doColorShift = true;
	colorShiftColor = "1 0 0 1";
	image = hammerImage;

	canDrop = true;
};

// #1.3
datablock fxDTSBrickData(brick16x16Data)
{
	brickFile = "./16x16.blb";
	category = "Bricks";
	subCategory = "16x";
	uiName = "16x16";
	iconName = "Add-Ons/GameMode_ReverieFortWars/models/brick16x16";
};

// #2.
// #2.1
datablock AudioProfile(swordDrawSound)
{
	filename = "Add-Ons/Weapon_Sword/swordDraw.wav";
	description = AudioClosest3d;
	preload = true;
};

datablock AudioProfile(swordHitSound)
{
	filename = "Add-Ons/Weapon_Sword/swordHit.wav";
	description = AudioClosest3d;
	preload = true;
};

datablock ParticleData(swordExplosionParticle)
{
	dragCoefficient = 4.5;
	gravityCoefficient = 3.5;
	inheritedVelFactor = 0.4;
	constantAcceleration = 0;
	spinRandomMin = -90;
	spinRandomMax = 90;
	lifetimeMS = 350;
	lifetimeVarianceMS = 300;
	textureName = "base/data/particles/chunk";
	colors[0] = "0.88 0.76 0.0 1.0";
	colors[1] = "0.88 0.76 0.0 1.0";
	sizes[0] = 0.1;
	sizes[1] = 0.0;
};

datablock ParticleEmitterData(swordExplosionEmitter)
{
	ejectionPeriodMS = 8;
	periodVarianceMS = 0;
	ejectionVelocity = 15;
	velocityVariance = 7.5;
	ejectionOffset = 0.0;
	thetaMin = 0;
	thetaMax = 50;
	phiReferenceVel= 0;
	phiVariance= 360;
	overrideAdvance = false;
	particles = "swordExplosionParticle";

	uiName = "Sword Hit";
};

datablock ExplosionData(swordExplosion)
{
	lifeTimeMS = 200;

	//soundProfile = swordHitSound;

	particleEmitter = swordExplosionEmitter;
	particleDensity = 25;
	particleRadius = 0.2;

	faceViewer = true;
	explosionScale = "1 1 1";

	shakeCamera = true;
	camShakeFreq = "20.0 22.0 20.0";
	camShakeAmp = "0.2 0.2 0.2";
	camShakeDuration = 0.5;
	camShakeRadius = 5.0;

	lightStartRadius = 3;
	lightEndRadius = 0;
	lightStartColor = "0.88 0.76 0.21";
	lightEndColor = "0 0 0";
};

datablock ProjectileData(swordProjectile)
{
	directDamage = 35;
	directDamageType = $DamageType::Sword;
	radiusDamageType = $DamageType::Sword;
	explosion = swordExplosion;

	muzzleVelocity = 50;
	velInheritFactor = 1;

	armingDelay = 0;
	lifetime = 100;
	fadeDelay = 70;
	bounceElasticity = 0;
	bounceFriction = 0;
	isBallistic = false;
	gravityMod = 0;

	hasLight = false;
	lightRadius = 3;
	lightColor= "0.88 0.76 0.21";

	uiName = "Sword Hit";
};

function ShapeBaseImageData::DoMeleeStrike(%this, %obj, %slot, %angle, %range, %damage, %proj, %sound, %backstab)
{
	%scale = getWord(%obj.getScale(), 2);
	%start = %obj.getEyePoint();
	%targets = conalRaycastM(%start, %obj.getEyeVector(), %range * %scale, %angle, $Typemasks::PlayerObjectType | $Typemasks::CorpseObjectType, %obj);
	for(%i = 0; (%hit = getWord(%targets, %i)) !$= ""; %i++)
	{
		if(%hit.isDead)
		{
			if(isObject(%proj))
			{
				%hit.spawnExplosion(%proj, %scale);
			}
			%hit.butcher(%obj.client.account);
			%hitcount++;
		}
		else if(minigameCanDamage(%obj, %hit))
		{
			if(isObject(%proj))
			{
				%hit.spawnExplosion(%proj, %scale);
			}
			if(isObject(%sound))
			{
				serverPlay3D(%sound, %hit.getPosition());
			}
			if(minigameCanDamage(%obj, %hit))
			{
				if(%backstab && vectorDot(%obj.getForwardVector(), %hit.getForwardVector()) > 0.25)
				{
					%hit.damage(%obj, %hit.getPosition(), 2 * %damage * %scale, %this.damageType);
				}
				else
				{
					%hit.damage(%obj, %hit.getPosition(), %damage * %scale, %this.damageType);
				}
			}
			%hitcount++;
		}
	}
	if(%hitcount == 0)
	{
		%typemasks = $Typemasks::VehicleObjectType | $Typemasks::TerrainObjectType | $Typemasks::FXbrickAlwaysObjectType | $TypeMasks::StaticShapeObjectType;
		%end = vectorAdd(%start, vectorScale(%obj.getMuzzleVector(%slot), %range * %scale));
		%raycast = containerRaycast(%start, %end, %typemasks, %obj);
		while(isObject(%hit = getWord(%raycast, 0)))
		{
			%crashCheck++;
			if(%crashCheck > 20)
			{
				error("DoMeleeStrike crash check failed");
				break;
			}
			%pos = posFromRaycast(%raycast);
			%normal = normalFromRaycast(%raycast);
			if(%hit.getClassName() $= "fxDtsBrick" && %this.isPickaxe)
			{
				if(%hit.isColliding())
				{
					%hit.damage(%obj, %damage * 5 * %scale, %this.tier, %pos, %normal);
				}
				else
				{
					%start = posFromRaycast(%raycast);
					if(%start $= "")
					{
						break;
					}
					else
					{
						%raycast = containerRaycast(posFromRaycast(%raycast), %end, %typemasks, %hit);
						%hit = firstWord(%raycast);
						continue;
					}
				}
			}
			if(isObject(%proj))
			{
				%p = new Projectile()
				{
					datablock = %proj;
					initialPosition = %pos;
					initialVelocity = %normal;
					scale = %scale SPC %scale SPC %scale;
					sourceSlot = %slot;
					sourceObject = %obj;
				};
				%p.explode();
			}
			if(isObject(%sound))
			{
				serverPlay3D(%sound, posFromRaycast(%raycast));
			}
			break;
		}
	}
}

// #2.2
datablock ItemData(fistsItem)
{
	category = "Weapon";
	className = "Weapon";

	shapeFile = "Add-Ons/Gamemode_Rampage/rHand.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	uiName = "Fists";
	iconName = "Add-Ons/Gamemode_ReverieFortWars/models/icon_Fists";
	doColorShift = true;
	colorShiftColor = "1 1 1 0.7";

	image = fistsImage;
	canDrop = false;
};

datablock ShapeBaseImageData(fistsImage)
{
	shapeFile = fistsItem.shapeFile;
	emap = true;

	mountPoint = 0;
	offset = "0 0 0";

	correctMuzzleVector = false;

	eyeOffset = "0 0 0";

	className = "WeaponImage";

	item = fistsItem;
	ammo = " ";
	projectile = "";
	projectileType = Projectile;

	melee = true;
	doRetraction = false;
	armReady = true;
	isPickaxe = true;

	doColorShift = true;
	colorShiftColor = fistsItem.colorShiftColor;

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.5;
	stateTransitionOnTimeout[0] = "Ready";

	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "PreFire";
	stateAllowImageChange[1] = true;

	stateName[2] = "PreFire";
	stateScript[2] = "onPreFire";
	stateAllowImageChange[2] = false;
	stateTimeoutValue[2] = 0.1;
	stateTransitionOnTimeout[2] = "Fire";

	stateName[3] = "Fire";
	stateTransitionOnTimeout[3] = "StopFire";
	stateTimeoutValue[3] = 0.2;
	stateFire[3] = true;
	stateAllowImageChange[3] = false;
	stateSequence[3] = "Fire";
	stateScript[3] = "onFire";
	stateWaitForTimeout[3] = true;

	stateName[5] = "StopFire";
	stateTransitionOnTimeout[5] = "Ready";
	stateTimeoutValue[5] = 0.1;
	stateAllowImageChange[5] = false;
	stateWaitForTimeout[5] = true;
	stateSequence[5] = "StopFire";
	stateScript[5] = "onStopFire";
};

function fistsImage::onPreFire(%this, %obj, %slot)
{
	%obj.playthread(2, armattack);
}

function fistsImage::onFire(%this, %obj, %slot)
{
	%this.DoMeleeStrike(%obj, %slot, 15, 3, 6, %this.projectile);
}

function fistsImage::onStopFire(%this, %obj, %slot)
{
	%obj.playthread(2, root);
}

// #2.3
// Wooden Sword declaration
datablock ItemData(WoodenSwordItem)
{
	category = "Weapon";
	className = "Weapon";

	shapeFile = "Add-Ons/Gamemode_ReverieFortWars/models/sword-2.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	uiName = "Wooden Sword";
	iconName = "Add-Ons/Gamemode_ReverieFortWars/models/icon_sword-2";
	doColorShift = true;
	colorShiftColor = "0.81 0.69 0.56 1";

	image = woodenSwordImage;
	canDrop = true;
};

datablock ShapeBaseImageData(WoodenSwordImage)
{
	shapeFile = woodenSwordItem.shapeFile;
	emap = true;

	mountPoint = 0;
	offset = "0 0 0";

	correctMuzzleVector = false;

	eyeOffset = "0 0 0";

	className = "WeaponImage";

	item = WoodenSwordItem;
	tier = 0;
	ammo = " ";
	projectile = swordProjectile;
	damageType = $DamageType::Sword;
	soundProfile = swordHitSound;
	projectileType = Projectile;

	melee = true;
	doRetraction = false;
	armReady = true;

	doColorShift = true;
	colorShiftColor = WoodenSwordItem.colorShiftColor;

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.5;
	stateTransitionOnTimeout[0] = "Ready";
	stateSound[0] = weaponSwitchSound;

	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "PreFire";
	stateAllowImageChange[1] = true;

	stateName[2] = "PreFire";
	stateScript[2] = "onPreFire";
	stateAllowImageChange[2] = false;
	stateTimeoutValue[2] = 0.1;
	stateTransitionOnTimeout[2] = "Fire";

	stateName[3] = "Fire";
	stateTransitionOnTimeout[3] = "CheckFire";
	stateTimeoutValue[3]= 0.2;
	stateFire[3] = true;
	stateAllowImageChange[3] = false;
	stateSequence[3] = "Fire";
	stateScript[3] = "onFire";
	stateWaitForTimeout[3] = true;

	stateName[4] = "CheckFire";
	stateTransitionOnTriggerUp[4]	= "StopFire";
	stateTransitionOnTriggerDown[4] = "Fire";

	stateName[5] = "StopFire";
	stateTransitionOnTimeout[5] = "Ready";
	stateTimeoutValue[5] = 0.2;
	stateAllowImageChange[5] = false;
	stateWaitForTimeout[5] = true;
	stateSequence[5] = "StopFire";
	stateScript[5] = "onStopFire";
};

function woodenSwordImage::onPreFire(%this, %obj, %slot)
{
	%obj.playthread(2, armattack);
}

function woodenSwordImage::onStopFire(%this, %obj, %slot)
{	
	%obj.playthread(2, root);
}

function woodenSwordImage::onFire(%this, %obj, %slot)
{
	%this.DoMeleeStrike(%obj, %slot, 10, 3, 10, %this.projectile, swordHitSound);
}

// Stone Sword declaration
datablock ItemData(StoneSwordItem : WoodenSwordItem)
{
	shapeFile = "Add-Ons/Gamemode_ReverieFortWars/models/sword-1.dts";
	iconName = "Add-Ons/Gamemode_ReverieFortWars/models/icon_sword-1";
	colorShiftColor = "0.5 0.5 0.5 1";
	uiName = "Stone Sword";
	image = StoneSwordImage;
};

datablock ShapeBaseImageData(StoneSwordImage : WoodenSwordImage)
{
	shapeFile = stoneSwordItem.shapeFile;
	colorShiftcolor = stoneSwordItem.colorShiftColor;
	tier = 1;
	item = stoneSwordItem;
	isPickaxe = false;
};

function StoneSwordImage::onPreFire(%this, %obj, %slot)
{
	%obj.playthread(2, armattack);
}

function StoneSwordImage::onStopFire(%this, %obj, %slot)
{	
	%obj.playthread(2, root);
}

function StoneSwordImage::onFire(%this, %obj, %slot)
{
	%this.DoMeleeStrike(%obj, %slot, 10, 3, 12, %this.projectile, swordHitSound);
}

// Bone Sword declaration
datablock ItemData(BoneSwordItem : WoodenSwordItem)
{
	shapeFile = "Add-Ons/Gamemode_ReverieFortWars/models/sword-1.dts";
	iconName = "Add-Ons/Gamemode_ReverieFortWars/models/icon_sword-1";
	colorShiftColor = "0.8 0.8 0.8 1 1";
	uiName = "Bone Sword";
	image = BoneSwordImage;
};

datablock ShapeBaseImageData(BoneSwordImage : WoodenSwordImage)
{
	shapeFile = BoneSwordItem.shapeFile;
	colorShiftcolor = BoneSwordItem.colorShiftColor;
	tier = 1;
	item = BoneSwordItem;
	isPickaxe = false;
};

function BoneSwordImage::onPreFire(%this, %obj, %slot)
{
	%obj.playthread(2, armattack);
}

function BoneSwordImage::onStopFire(%this, %obj, %slot)
{	
	%obj.playthread(2, root);
}

function BoneSwordImage::onFire(%this, %obj, %slot)
{
	%this.DoMeleeStrike(%obj, %slot, 10, 3, 12, %this.projectile, swordHitSound);
}

// Obsidian Sword declaration / Macahuitl decalaration
datablock ItemData(MacahuitlItem : WoodenSwordItem)
{
	shapeFile = "Add-Ons/Gamemode_ReverieFortWars/models/Macahuitl.dts";
	iconName = "Add-Ons/Gamemode_ReverieFortWars/models/icon_Macahuitl";
	colorShiftColor = woodenSwordItem.colorShiftColor;
	uiName = "Obsidian Sword";
	image = MacahuitlImage;
};

datablock ShapeBaseImageData(MacahuitlImage : WoodenSwordImage)
{
	shapeFile = MacahuitlItem.shapeFile;
	colorShiftcolor = MacahuitlItem.colorShiftColor;
	tier = 2;
	item = MacahuitlItem;
	isPickaxe = false;
};

function MacahuitlImage::onPreFire(%this, %obj, %slot)
{
	%obj.playthread(2, armattack);
}

function MacahuitlImage::onStopFire(%this, %obj, %slot)
{	
	%obj.playthread(2, root);
}

function MacahuitlImage::onFire(%this, %obj, %slot)
{
	%this.DoMeleeStrike(%obj, %slot, 10, 3, getRandom() < 0.2 ? 30 : 15, %this.projectile);
}

// Surtra Sword declaration
datablock ItemData(SurtraSwordItem : WoodenSwordItem)
{
	shapeFile = "Add-Ons/Gamemode_ReverieFortWars/models/sword-1.dts";
	iconName = "Add-Ons/Gamemode_ReverieFortWars/models/icon_sword-1";
	colorShiftColor = "0.31 0.29 0.25 1";
	uiName = "Surtra Sword";
	image = SurtraSwordImage;
};

datablock ShapeBaseImageData(SurtraSwordImage : WoodenSwordImage)
{
	shapeFile = SurtraSwordItem.shapeFile;
	colorShiftcolor = SurtraSwordItem.colorShiftColor;
	tier = 2;
	item = SurtraSwordItem;
	isPickaxe = false;
	stateSound[0] = swordDrawSound;
};

function SurtraSwordImage::onPreFire(%this, %obj, %slot)
{
	%obj.playthread(2, armattack);
}

function SurtraSwordImage::onStopFire(%this, %obj, %slot)
{	
	%obj.playthread(2, root);
}

function SurtraSwordImage::onFire(%this, %obj, %slot)
{
	%this.DoMeleeStrike(%obj, %slot, 10, 3, 15, %this.projectile, swordHitSound);
}

// Calcite Sword declaration
datablock ItemData(CalciteSwordItem : WoodenSwordItem)
{
	shapeFile = "Add-Ons/Weapon_Sword/sword.dts";
	iconName = "Add-Ons/Gamemode_ReverieFortWars/models/icon_sword";
	colorShiftColor = "0.87 0.8 0.57 1";
	uiName = "Calcite Sword";
	image = CalciteSwordImage;
};

datablock ShapeBaseImageData(CalciteSwordImage : WoodenSwordImage)
{
	shapeFile = CalciteSwordItem.shapeFile;
	colorShiftcolor = CalciteSwordItem.colorShiftColor;
	tier = 3;
	item = CalciteSwordItem;
	isPickaxe = false;
	stateSound[0] = swordDrawSound;
};

function CalciteSwordImage::onPreFire(%this, %obj, %slot)
{
	%obj.playthread(2, armattack);
}

function CalciteSwordImage::onStopFire(%this, %obj, %slot)
{	
	%obj.playthread(2, root);
}

function CalciteSwordImage::onFire(%this, %obj, %slot)
{
	%this.DoMeleeStrike(%obj, %slot, 10, 3, 20, %this.projectile, swordHitSound);
}

// Nithlite Sword declaration
datablock ItemData(NithliteSwordItem : WoodenSwordItem)
{
	shapeFile = "Add-Ons/Gamemode_ReverieFortWars/models/swordalt.dts";
	iconName = "Add-Ons/Gamemode_ReverieFortWars/models/icon_swordalt";
	colorShiftColor = "0.96 0.75 0.89 1";
	uiName = "Nithlite Sword";
	image = NithliteSwordImage;
};

datablock ShapeBaseImageData(NithliteSwordImage : WoodenSwordImage)
{
	shapeFile = NithliteSwordItem.shapeFile;
	colorShiftcolor = NithliteSwordItem.colorShiftColor;
	tier = 3;
	item = NithliteSwordItem;
	isPickaxe = false;
	stateSound[0] = swordDrawSound;
};

function NithliteSwordImage::onPreFire(%this, %obj, %slot)
{
	%obj.playthread(2, armattack);
}

function NithliteSwordImage::onStopFire(%this, %obj, %slot)
{	
	%obj.playthread(2, root);
}

function NithliteSwordImage::onFire(%this, %obj, %slot)
{
	%this.DoMeleeStrike(%obj, %slot, 12, 3, 20, %this.projectile, swordHitSound);
}

// Sanguite Sword declaration
datablock ItemData(SanguiteSwordItem : WoodenSwordItem)
{
	shapeFile = "Add-Ons/Gamemode_ReverieFortWars/models/sword+1.dts";
	iconName = "Add-Ons/Gamemode_ReverieFortWars/models/icon_sword+1";
	colorShiftColor = "0.56 0.03 0.03 1";
	uiName = "Sanguite Sword";
	image = SanguiteSwordImage;
};

datablock ShapeBaseImageData(SanguiteSwordImage : WoodenSwordImage)
{
	shapeFile = SanguiteSwordItem.shapeFile;
	colorShiftcolor = SanguiteSwordItem.colorShiftColor;
	tier = 4;
	item = SanguiteSwordItem;
	isPickaxe = false;
	stateSound[0] = swordDrawSound;
};

function SanguiteSwordImage::onPreFire(%this, %obj, %slot)
{
	%obj.playthread(2, armattack);
}

function SanguiteSwordImage::onStopFire(%this, %obj, %slot)
{	
	%obj.playthread(2, root);
}

function SanguiteSwordImage::onFire(%this, %obj, %slot)
{
	%this.DoMeleeStrike(%obj, %slot, 15, 3, 25, %this.projectile, swordHitSound);
}

// Sinnite Sword declaration
datablock ItemData(SinniteSwordItem : WoodenSwordItem)
{
	shapeFile = "Add-Ons/Gamemode_ReverieFortWars/models/sword+1.dts";
	iconName = "Add-Ons/Gamemode_ReverieFortWars/models/icon_sword+1";
	colorShiftColor = "0.54 0.50 0.43 1";
	uiName = "Sinnite Sword";
	image = SinniteSwordImage;
};

datablock ShapeBaseImageData(SinniteSwordImage : WoodenSwordImage)
{
	shapeFile = SinniteSwordItem.shapeFile;
	colorShiftcolor = SinniteSwordItem.colorShiftColor;
	tier = 4;
	item = SinniteSwordItem;
	isPickaxe = false;
	stateSound[0] = swordDrawSound;
};

function SinniteSwordImage::onPreFire(%this, %obj, %slot)
{
	%obj.playthread(2, armattack);
}

function SinniteSwordImage::onStopFire(%this, %obj, %slot)
{	
	%obj.playthread(2, root);
}

function SinniteSwordImage::onFire(%this, %obj, %slot)
{
	%this.DoMeleeStrike(%obj, %slot, 18, 3, 25, %this.projectile, swordHitSound);
}

// Sinnite Sword declaration
datablock ItemData(CrystalSwordItem : WoodenSwordItem)
{
	shapeFile = "Add-Ons/Gamemode_ReverieFortWars/models/sword+1.dts";
	iconName = "Add-Ons/Gamemode_ReverieFortWars/models/icon_sword+1";
	colorShiftColor = getColorIdTable($RFW::ResourceColor["Crystal"]);
	uiName = "Crystal Sword";
	image = CrystalSwordImage;
};

datablock ShapeBaseImageData(CrystalSwordImage : WoodenSwordImage)
{
	shapeFile = CrystalSwordItem.shapeFile;
	colorShiftcolor = CrystalSwordItem.colorShiftColor;
	tier = 5;
	item = CrystalSwordItem;
	isPickaxe = false;
	stateSound[0] = swordDrawSound;
};

function CrystalSwordImage::onPreFire(%this, %obj, %slot)
{
	%obj.playthread(2, armattack);
}

function CrystalSwordImage::onStopFire(%this, %obj, %slot)
{	
	%obj.playthread(2, root);
}

function CrystalSwordImage::onFire(%this, %obj, %slot)
{
	%this.DoMeleeStrike(%obj, %slot, 20, 3.5, 25, %this.projectile, swordHitSound);
}

// #2.4

// zzz why did I ever do this
function RFW_RegisterWeapon(%dbName, %uiName, %shapeFile, %iconName, %colorshiftColor, %tier, %hitAngle, %range, %damage, %isPickaxe, %damageType, %sound, %projectile)
{
	eval("datablock ItemData(" @ %dbName @ "Item : woodenSwordItem) { colorShiftColor = \"" @ %colorshiftColor @ "\"; shapeFile = \"" @ %shapeFile @ "\"; iconName = \"" @ %iconName @ "\"; uiName = \"" @ %uiName @ "\"; image = " @ %dbName @ "Image; };");
	eval("datablock ShapeBaseImageData(" @ %dbName @ "Image : woodenSwordImage) { colorShiftColor = \"" @ %colorShiftColor @ "\"; shapeFile = \"" @ %shapeFile @ "\"; item = " @ %dbName @ "Item; tier = " @ %tier @ "; isPickaxe = " @ %isPickaxe @ "; stateSound[0] = " @ (%isPickaxe ? weaponSwitchSound : swordDrawSound) @ "; };");
	if(%damageType !$= "")
	{
		eval(%dbName @ "image.damageType = " @ %damageType @ ";");
	}
	if(%sound !$= "")
	{
		eval(%dbName @ "image.soundProfile = " @ %sound @ ";");
	}
	if(%projectile !$= "")
	{
		eval(%dbName @ "image.projectile = " @ %projectile @ ";");
	}
	eval("function " @ %dbName @ "Image::onPreFire(%this, %obj, %slot) { %obj.playThread(2, armAttack); }");
	eval("function " @ %dbName @ "Image::onStopFire(%this, %obj, %slot) { %obj.playThread(2, root); }");
	eval("function " @ %dbName @ "Image::onFire(%this, %obj, %slot) { %this.DoMeleeStrike(%obj, %slot, " @ %hitAngle @ ", " @ %range @ ", " @ %damage @ ", %this.projectile); }");
}

RFW_RegisterWeapon("stonePickaxe", "Stone Pickaxe", "Add-Ons/Gamemode_ReverieFortWars/models/stonepickaxe.dts", "Add-Ons/Gamemode_ReverieFortWars/models/StonePickaxe", "0.5 0.5 0.5 1", 1, 15, 3.5, 10, true, $DamageType::Pickaxe, hammerHitSound);
RFW_RegisterWeapon("surtraPickaxe", "Surtra Pickaxe", "Add-Ons/Gamemode_ReverieFortWars/models/surtrapickaxe.dts", "Add-Ons/Gamemode_ReverieFortWars/models/SurtraPickaxe", "0.31 0.29 0.25 1", 2, 15, 3.5, 13, true, $DamageType::Pickaxe, hammerHitSound);
RFW_RegisterWeapon("calcitePickaxe", "Calcite Pickaxe", "Add-Ons/Gamemode_ReverieFortWars/models/calcitepickaxe.dts", "Add-Ons/Gamemode_ReverieFortWars/models/CalcitePickaxe", "0.87 0.8 0.57 1", 3, 15, 3.5, 16, true, $DamageType::Pickaxe, hammerHitSound);
RFW_RegisterWeapon("nithlitePickaxe", "Nithlite Pickaxe", "Add-Ons/Gamemode_ReverieFortWars/models/nithlitepickaxe.dts", "Add-Ons/Gamemode_ReverieFortWars/models/NithlitePickaxe", "0.96 0.75 0.89 1", 3, 15, 3.5, 16, true, $DamageType::Pickaxe, hammerHitSound);
RFW_RegisterWeapon("sanguitePickaxe", "Sanguite Pickaxe", "Add-Ons/Gamemode_ReverieFortWars/models/sanguitepickaxe.dts", "Add-Ons/Gamemode_ReverieFortWars/models/SanguitePickaxe", "0.56 0.03 0.03 1", 4, 20, 3.5, 20, true, $DamageType::Pickaxe, hammerHitSound);
RFW_RegisterWeapon("sinnitePickaxe", "Sinnite Pickaxe", "Add-Ons/Gamemode_ReverieFortWars/models/sinnitepickaxe.dts", "Add-Ons/Gamemode_ReverieFortWars/models/SinnitePickaxe", "0.54 0.50 0.43 1", 4, 20, 3.5, 20, true, $DamageType::Pickaxe, hammerHitSound);
RFW_RegisterWeapon("crystalPickaxe", "Crystal Pickaxe", "Add-Ons/Gamemode_ReverieFortWars/models/sinnitepickaxe.dts", "Add-Ons/Gamemode_ReverieFortWars/models/SinnitePickaxe", setWord(crystalSwordItem.colorShiftColor, 3, 1), 4, 20, 3.5, 25, true, $DamageType::Pickaxe, hammerHitSound);

// #2.5

datablock ItemData(SurtraDaggerItem)
{
	category = "Weapon";
	className = "Weapon";

	shapeFile = "Add-Ons/Gamemode_ReverieFortWars/models/dagger-1.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	uiName = "Surtra dagger";
	iconName = "Add-Ons/Gamemode_ReverieFortWars/models/icon_dagger-1";
	doColorShift = true;
	colorShiftColor = SurtraSwordItem.colorShiftColor;

	image = surtraDaggerImage;
	canDrop = true;
};

datablock ShapeBaseImageData(surtraDaggerImage)
{
	shapeFile = surtraDaggerItem.shapeFile;
	emap = true;

	mountPoint = 0;
	offset = "0 0 0";

	correctMuzzleVector = false;

	eyeOffset = "0 0 0";

	className = "WeaponImage";

	item = surtraDaggerItem;
	tier = 0;
	ammo = " ";
	projectile = swordProjectile;
	damageType = $DamageType::Dagger;
	soundProfile = swordHitSound;
	projectileType = Projectile;

	melee = true;
	doRetraction = false;
	armReady = true;

	doColorShift = true;
	colorShiftColor = surtraDaggerItem.colorShiftColor;

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.1;
	stateTransitionOnTimeout[0]	= "Ready";
	stateSequence[0] = "ready";

	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "Charge";
	stateAllowImageChange[1] = true;
	
	stateName[2] = "Charge";
	stateTransitionOnTimeout[2] = "Armed";
	stateTimeoutValue[2] = 0.7;
	stateWaitForTimeout[2] = false;
	stateTransitionOnTriggerUp[2] = "AbortCharge";
	stateScript[2] = "onCharge";
	stateAllowImageChange[2] = false;
	
	stateName[3] = "AbortCharge";
	stateTransitionOnTimeout[3] = "Ready";
	stateTimeoutValue[3] = 0.3;
	stateWaitForTimeout[3] = true;
	stateScript[3] = "onAbortCharge";
	stateAllowImageChange[3] = false;

	stateName[4] = "Armed";
	stateTransitionOnTriggerUp[4] = "Fire";
	stateAllowImageChange[4] = false;

	stateName[5] = "Fire";
	stateTransitionOnTimeout[5] = "Ready";
	stateTimeoutValue[5] = 0.5;
	stateFire[5] = true;
	stateSequence[5] = "fire";
	stateScript[5] = "onFire";
	stateWaitForTimeout[5] = true;
	stateAllowImageChange[5] = false;
	stateSound[5] = spearFireSound;
};

function surtraDaggerImage::onCharge(%this, %obj, %slot)
{
	%obj.playthread(2, spearReady);
}

function surtraDaggerImage::onAbortCharge(%this, %obj, %slot)
{
	%obj.playthread(2, spearThrow);
	%this.DoMeleeStrike(%obj, %slot, 15, 3, 10, %this.projectile, swordHitSound, true);
}

function surtraDaggerImage::onFire(%this, %obj, %slot)
{
	%obj.playthread(2, spearThrow);
	%this.DoMeleeStrike(%obj, %slot, 15, 3, 30, %this.projectile, swordHitSound, true);
}

datablock ItemData(ObsidianDaggerItem : SurtraDaggerItem)
{
	shapeFile = "Add-Ons/Gamemode_ReverieFortWars/models/dagger-1.dts";
	iconName = "Add-Ons/Gamemode_ReverieFortWars/models/icon_dagger-1";
	uiName = "Obsidian dagger";
	colorShiftColor = "0.1 0.1 0.1 1";
	image = ObsidianDaggerImage;
};

datablock ShapeBaseImageData(ObsidianDaggerImage : SurtraDaggerImage)
{
	shapeFile = ObsidianDaggerItem.shapeFile;
	item = ObsidianDaggerItem;
	colorShiftColor = ObsidianDaggerItem.colorShiftColor;
};

function ObsidianDaggerImage::onCharge(%this, %obj, %slot)
{
	%obj.playthread(2, spearReady);
}

function ObsidianDaggerImage::onAbortCharge(%this, %obj, %slot)
{
	%obj.playthread(2, spearThrow);
	%this.DoMeleeStrike(%obj, %slot, 15, 3, 10, %this.projectile, swordHitSound, true);
}

function ObsidianDaggerImage::onFire(%this, %obj, %slot)
{
	%obj.playthread(2, spearThrow);
	%this.DoMeleeStrike(%obj, %slot, 15, 3, 30, %this.projectile, swordHitSound, true);
}

datablock ItemData(CalciteDaggerItem : SurtraDaggerItem)
{
	shapeFile = "Add-Ons/Gamemode_ReverieFortWars/models/dagger-1.dts";
	iconName = "Add-Ons/Gamemode_ReverieFortWars/models/icon_dagger-1";
	uiName = "Calcite dagger";
	colorShiftColor = CalciteSwordItem.colorShiftColor;
	image = CalciteDaggerImage;
};

datablock ShapeBaseImageData(CalciteDaggerImage : SurtraDaggerImage)
{
	shapeFile = CalciteDaggerItem.shapeFile;
	item = CalciteDaggerItem;
	colorShiftColor = CalciteDaggerItem.colorShiftColor;
};

function CalciteDaggerImage::onCharge(%this, %obj, %slot)
{
	%obj.playthread(2, spearReady);
}

function CalciteDaggerImage::onAbortCharge(%this, %obj, %slot)
{
	%obj.playthread(2, spearThrow);
	%this.DoMeleeStrike(%obj, %slot, 20, 3, 15, %this.projectile, swordHitSound, true);
}

function CalciteDaggerImage::onFire(%this, %obj, %slot)
{
	%obj.playthread(2, spearThrow);
	%this.DoMeleeStrike(%obj, %slot, 20, 3, 45, %this.projectile, swordHitSound, true);
}

datablock ItemData(NithliteDaggerItem : SurtraDaggerItem)
{
	shapeFile = "Add-Ons/Gamemode_ReverieFortWars/models/dagger-1.dts";
	iconName = "Add-Ons/Gamemode_ReverieFortWars/models/icon_dagger-1";
	uiName = "Nithlite dagger";
	colorShiftColor = NithliteSwordItem.colorShiftColor;
	image = NithliteDaggerImage;
};

datablock ShapeBaseImageData(NithliteDaggerImage : SurtraDaggerImage)
{
	shapeFile = NithliteDaggerItem.shapeFile;
	item = NithliteDaggerItem;
	colorShiftColor = NithliteDaggerItem.colorShiftColor;
};

function NithliteDaggerImage::onCharge(%this, %obj, %slot)
{
	%obj.playthread(2, spearReady);
}

function NithliteDaggerImage::onAbortCharge(%this, %obj, %slot)
{
	%obj.playthread(2, spearThrow);
	%this.DoMeleeStrike(%obj, %slot, 20, 3, 15, %this.projectile, swordHitSound, true);
}

function NithliteDaggerImage::onFire(%this, %obj, %slot)
{
	%obj.playthread(2, spearThrow);
	%this.DoMeleeStrike(%obj, %slot, 20, 3, 45, %this.projectile, swordHitSound, true);
}

datablock ItemData(SanguiteDaggerItem : SurtraDaggerItem)
{
	shapeFile = "Add-Ons/Gamemode_ReverieFortWars/models/dagger-1.dts";
	iconName = "Add-Ons/Gamemode_ReverieFortWars/models/icon_dagger-1";
	uiName = "Sanguite dagger";
	colorShiftColor = SanguiteSwordItem.colorShiftColor;
	image = SanguiteDaggerImage;
};

datablock ShapeBaseImageData(SanguiteDaggerImage : SurtraDaggerImage)
{
	shapeFile = SanguiteDaggerItem.shapeFile;
	item = SanguiteDaggerItem;
	colorShiftColor = SanguiteDaggerItem.colorShiftColor;
};

function SanguiteDaggerImage::onCharge(%this, %obj, %slot)
{
	%obj.playthread(2, spearReady);
}

function SanguiteDaggerImage::onAbortCharge(%this, %obj, %slot)
{
	%obj.playthread(2, spearThrow);
	%this.DoMeleeStrike(%obj, %slot, 25, 3.5, 20, %this.projectile, swordHitSound, true);
}

function SanguiteDaggerImage::onFire(%this, %obj, %slot)
{
	%obj.playthread(2, spearThrow);
	%this.DoMeleeStrike(%obj, %slot, 25, 3.5, 60, %this.projectile, swordHitSound, true);
}

datablock ItemData(SinniteDaggerItem : SurtraDaggerItem)
{
	shapeFile = "Add-Ons/Gamemode_ReverieFortWars/models/dagger-1.dts";
	iconName = "Add-Ons/Gamemode_ReverieFortWars/models/icon_dagger-1";
	uiName = "Sinnite dagger";
	colorShiftColor = SinniteSwordItem.colorShiftColor;
	image = SinniteDaggerImage;
};

datablock ShapeBaseImageData(SinniteDaggerImage : SurtraDaggerImage)
{
	shapeFile = SinniteDaggerItem.shapeFile;
	item = SinniteDaggerItem;
	colorShiftColor = SinniteDaggerItem.colorShiftColor;
};

function SinniteDaggerImage::onCharge(%this, %obj, %slot)
{
	%obj.playthread(2, spearReady);
}

function SinniteDaggerImage::onAbortCharge(%this, %obj, %slot)
{
	%obj.playthread(2, spearThrow);
	%this.DoMeleeStrike(%obj, %slot, 25, 3.5, 20, %this.projectile, swordHitSound, true);
}

function SinniteDaggerImage::onFire(%this, %obj, %slot)
{
	%obj.playthread(2, spearThrow);
	%this.DoMeleeStrike(%obj, %slot, 25, 3.5, 60, %this.projectile, swordHitSound, true);
}

datablock ItemData(CrystalDaggerItem : SurtraDaggerItem)
{
	shapeFile = "Add-Ons/Gamemode_ReverieFortWars/models/dagger-1.dts";
	iconName = "Add-Ons/Gamemode_ReverieFortWars/models/icon_dagger-1";
	uiName = "Crystal dagger";
	colorShiftColor = CrystalSwordItem.colorShiftColor;
	image = CrystalDaggerImage;
};

datablock ShapeBaseImageData(CrystalDaggerImage : SurtraDaggerImage)
{
	shapeFile = CrystalDaggerItem.shapeFile;
	item = CrystalDaggerItem;
	colorShiftColor = CrystalDaggerItem.colorShiftColor;
};

function CrystalDaggerImage::onCharge(%this, %obj, %slot)
{
	%obj.playthread(2, spearReady);
}

function CrystalDaggerImage::onAbortCharge(%this, %obj, %slot)
{
	%obj.playthread(2, spearThrow);
	%this.DoMeleeStrike(%obj, %slot, 30, 3.5, 25, %this.projectile, swordHitSound, true);
}

function CrystalDaggerImage::onFire(%this, %obj, %slot)
{
	%obj.playthread(2, spearThrow);
	%this.DoMeleeStrike(%obj, %slot, 30, 3.5, 75, %this.projectile, swordHitSound, true);
}

// #3.
// #3.1
function DoBowShot(%obj, %slot, %headshot)
{
	if(isObject(%acc = %obj.client.account))
	{
		%quiver = %acc.read("Back");
		%type = getItemType(%quiver);
		switch$(%type)
		{
			case "Crystal quiver":
				%speed = 150;
				%damage = 25;
			case "Sanguite quiver":
				%speed = 120;
				%damage = 21;
			case "Sinnite quiver":
				%speed = 120;
				%damage = 21;
			case "Calcite quiver":
				%speed = 110;
				%damage = 19;
			case "Nithlite quiver":
				%speed = 110;
				%damage = 19;
			case "Obsidian quiver":
				%speed = 100;
				%damage = 17;
				%headshot = true;
			case "Surtra quiver":
				%speed = 100;
				%damage = 17;
			case "Stone quiver":
				%speed = 90;
				%damage = 15;
			case "Bone quiver":
				%speed = 90;
				%damage = 15;
			case "Wooden quiver":
				%speed = 80;
				%damage = 12;
			default:
				%obj.client.centerPrintQueue("<color:FF0000>Need to equip a quiver!", 3);
				return;
		}
		%vec = %obj.getMuzzleVector(%slot);
		%pos = vectorAdd(%obj.getMuzzlePoint(%slot), vectorScale(%vec, 0.1));
		%vec = vectorAdd(vectorScale(%vec, %speed), %obj.getVelocity());
		%proj = new Projectile()
		{
			datablock = arrowProjectile;
			initialPosition = %pos;
			initialVelocity = %vec;
			sourceObject = %obj;
			damage = %damage;
			headshot = %headshot;
		};
	}
	else
	{
		%vec = %obj.getMuzzleVector(%slot);
		%pos = vectorAdd(%obj.getMuzzlePoint(%slot), vectorScale(%vec, 0.1));
		%vec = vectorAdd(vectorScale(%vec, 90), %obj.getVelocity());
		%proj = new Projectile()
		{
			datablock = arrowProjectile;
			initialPosition = %pos;
			initialVelocity = %vec;
			sourceObject = %obj;
			damage = 15;
		};
	}
	return %proj;
}

datablock AudioProfile(arrowHitSound)
{
	filename = "Add-Ons/Weapon_Bow/arrowHit.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(bowFireSound)
{
	filename = "Add-Ons/Weapon_Bow/bowFire.wav";
	description = AudioClosest3d;
	preload = true;
};


datablock ParticleData(arrowTrailParticle)
{
	dragCoefficient = 3.0;
	windCoefficient = 0.0;
	gravityCoefficient = 0.0;
	inheritedVelFactor = 0.0;
	constantAcceleration = 0.0;
	lifetimeMS = 200;
	lifetimeVarianceMS = 0;
	spinSpeed = 10.0;
	spinRandomMin = -50.0;
	spinRandomMax = 50.0;
	useInvAlpha = false;
	animateTexture = false;

	textureName = "base/data/particles/dot";

	colors[0] = "1 1 1 0.2";
	colors[1]= "1 1 1 0.0";
	sizes[0] = 0.2;
	sizes[1] = 0.01;
	times[0] = 0.0;
	times[1] = 1.0;
};

datablock ParticleEmitterData(arrowTrailEmitter)
{
	ejectionPeriodMS = 2;
	periodVarianceMS = 0;

	ejectionVelocity = 0;
	velocityVariance = 0;

	ejectionOffset = 0;

	thetaMin = 0.0;
	thetaMax = 90.0;  

	particles = arrowTrailParticle;

	useEmitterColors = true;
	uiName = "Arrow Trail";
};

//effects
datablock ParticleData(arrowStickExplosionParticle)
{
	dragCoefficient = 5;
	gravityCoefficient = 0.1;
	inheritedVelFactor = 0.2;
	constantAcceleration = 0.0;
	lifetimeMS = 500;
	lifetimeVarianceMS = 300;
	textureName = "base/data/particles/chunk";
	spinSpeed = 10.0;
	spinRandomMin = -50.0;
	spinRandomMax = 50.0;
	colors[0] = "0.9 0.9 0.6 0.9";
	colors[1] = "0.9 0.5 0.6 0.0";
	sizes[0] = 0.25;
	sizes[1] = 0.0;
};
datablock ParticleEmitterData(arrowStickExplosionEmitter)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionVelocity = 5;
	velocityVariance = 0.0;
	ejectionOffset = 0.0;
	thetaMin = 80;
	thetaMax = 80;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = false;
	particles = "arrowStickExplosionParticle";

	useEmitterColors = true;
	uiName = "Arrow Stick";
};

datablock ExplosionData(arrowStickExplosion)
{
	soundProfile = arrowHitSound;

	lifeTimeMS = 150;

	particleEmitter = arrowStickExplosionEmitter;
	particleDensity = 10;
	particleRadius = 0.2;

	emitter[0] = "";

	faceViewer     = true;
	explosionScale = "1 1 1";

	shakeCamera = false;
	camShakeFreq = "10.0 11.0 10.0";
	camShakeAmp = "1.0 1.0 1.0";
	camShakeDuration = 0.5;
	camShakeRadius = 10.0;

	lightStartRadius = 0;
	lightEndRadius = 0;
	lightStartColor = "0 0 0";
	lightEndColor = "0 0 0";
};

datablock ParticleData(arrowExplosionParticle)
{
	dragCoefficient = 8;
	gravityCoefficient = -0.3;
	inheritedVelFactor = 0.2;
	constantAcceleration = 0.0;
	lifetimeMS = 500;
	lifetimeVarianceMS = 300;
	textureName = "base/data/particles/cloud";
	spinSpeed = 10.0;
	spinRandomMin = -50.0;
	spinRandomMax = 50.0;
	colors[0] = "0.5 0.5 0.5 0.9";
	colors[1] = "0.5 0.5 0.5 0.0";
	sizes[0] = 0.45;
	sizes[1] = 0.0;

   useInvAlpha = true;
};
datablock ParticleEmitterData(arrowExplosionEmitter)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionVelocity = 3;
	velocityVariance = 0.0;
	ejectionOffset = 0.0;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = false;
	particles = "arrowExplosionParticle";

	useEmitterColors = true;
	uiName = "Arrow Vanish";
};

datablock ExplosionData(arrowExplosion)
{
	soundProfile = "";

	lifeTimeMS = 50;

	emitter[0] = arrowExplosionEmitter;

	faceViewer     = true;
	explosionScale = "1 1 1";

	shakeCamera = false;
	camShakeFreq = "10.0 11.0 10.0";
	camShakeAmp = "1.0 1.0 1.0";
	camShakeDuration = 0.5;
	camShakeRadius = 10.0;

	lightStartRadius = 0;
	lightEndRadius = 0;
	lightStartColor = "0 0 0";
	lightEndColor = "0 0 0";
};

datablock ProjectileData(arrowProjectile)
{
	projectileShapeName = "Add-Ons/Weapon_Bow/arrow.dts";

	directDamage = 30;
	directDamageType = $DamageType::ArrowDirect;

	radiusDamage = 0;
	damageRadius = 0;
	radiusDamageType = $DamageType::ArrowDirect;

	explosion = arrowExplosion;
	stickExplosion = arrowStickExplosion;
	bloodExplosion = arrowStickExplosion;
	particleEmitter = arrowTrailEmitter;
	explodeOnPlayerImpact = true;
	explodeOnDeath = true;  

	armingDelay = 4000;
	lifetime = 4000;
	fadeDelay = 4000;

	isBallistic = true;
	bounceAngle = 170;
	minStickVelocity = 10;
	bounceElasticity = 0.2;
	bounceFriction = 0.01;   
	gravityMod = 0.25;

	hasLight = false;
	lightRadius = 3.0;
	lightColor = "0 0 0.5";

	muzzleVelocity = 65;
	velInheritFactor = 1;

	uiName = "Arrow";
};

function ArrowProjectile::onCollision(%this, %obj, %col, %fade, %pos, %normal)
{
	if(%col.getType() & $Typemasks::PlayerObjectType && minigameCanDamage(%obj.sourceObject, %col))
	{
		if(%obj.headShot)
		{
			%hitbox = getHitbox(%obj, %col, %pos);
			if(strPos(%hitbox, "headSkin") != -1)
			{
				%obj.damage*= 2;
			}
		}
		%col.damage(%obj.sourceObject, %pos, %obj.damage, $DamageType::Bow);
	}
}

datablock ItemData(WoodenBowItem)
{
	category = "Weapon";
	className = "Weapon";

	shapeFile = "Add-Ons/Weapon_Bow/bow.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	uiName = "Wooden Bow";
	iconName = "Add-Ons/Weapon_Bow/icon_bow";
	doColorShift = true;
	colorShiftColor = "0.81 0.69 0.56 1";

	image = WoodenBowImage;
	canDrop = true;
};

datablock ShapeBaseImageData(WoodenBowImage)
{
	shapeFile = "Add-Ons/Weapon_Bow/bow.dts";
	emap = true;

	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = 0;
	rotation = eulerToMatrix( "0 0 10" );

	correctMuzzleVector = true;

	className = "WeaponImage";

	item = BowItem;
	ammo = " ";
	projectile = "";
	projectileType = Projectile;

	melee = false;
	armReady = true;

	doColorShift = true;
	colorShiftColor = WoodenBowItem.colorShiftColor;

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.5;
	stateTransitionOnTimeout[0] = "Ready";
	stateSound[0] = weaponSwitchSound;

	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "Fire";
	stateAllowImageChange[1] = true;

	stateName[2] = "Fire";
	stateTransitionOnTimeout[2] = "Reload";
	stateTimeoutValue[2] = 0.05;
	stateFire[2] = true;
	stateAllowImageChange[2] = false;
	stateSequence[2] = "Fire";
	stateScript[2] = "onFire";
	stateWaitForTimeout[2] = true;
	stateSound[2] = bowFireSound;

	stateName[3] = "Reload";
	stateSequence[3] = "Reload";
	stateAllowImageChange[3] = false;
	stateTimeoutValue[3] = 0.5;
	stateWaitForTimeout[3] = true;
	stateTransitionOnTimeout[3] = "Check";

	stateName[4] = "Check";
	stateTransitionOnTriggerUp[4] = "StopFire";
	stateTransitionOnTriggerDown[4] = "Fire";

	stateName[5] = "StopFire";
	stateTransitionOnTimeout[5] = "Ready";
	stateTimeoutValue[5] = 0.2;
	stateAllowImageChange[5]= false;
	stateWaitForTimeout[5] = true;
	stateScript[5] = "onStopFire";
};

function WoodenBowImage::onFire(%this, %obj, %slot)
{
	DoBowShot(%obj, %slot, false);
}

datablock ItemData(BoneBowItem : WoodenBowItem)
{
	image = BoneBowImage;
	colorShiftColor = "0.8 0.8 0.8 1";
	uiName = "Bone Bow";
};

datablock ShapeBaseImageData(BoneBowImage : WoodenBowImage)
{
	item = BoneBowItem;
	colorShiftColor = "0.8 0.8 0.8 1";

	stateTimeoutValue[3] = 0.42;
};

function BoneBowImage::onFire(%this, %obj, %slot)
{
	DoBowShot(%obj, %slot, true);
}

datablock ItemData(FaerieBowItem : WoodenBowItem)
{
	image = FaerieBowImage;
	colorShiftColor = "0 0.7 0.3 1";
	uiName = "Faerie Bow";
};

datablock ShapeBaseImageData(FaerieBowImage : WoodenBowImage)
{
	item = FaerieBowItem;
	colorShiftColor = "0 0.7 0.3 1";

	stateTimeoutValue[3] = 0.42;
};

function FaerieBowImage::onFire(%this, %obj, %slot)
{
	%proj = DoBowShot(%obj, %slot, true);
	if(!isObject(%proj))
	{
		return;
	}
	%projData = %proj.getDatablock();
	%vel = vectorLen(%proj.getVelocity());
	//target info
	%start = %obj.getMuzzlePoint(%slot);
	%masks = $Typemasks::PlayerObjectType;
	%tgts = conalRaycastM(%start, %obj.getEyeVector(), 300, 7.5, %masks, %obj);
	while((%tgt = firstWord(%tgts)) !$= "")
	{
		if(minigameCanDamage(%obj, %tgt))
		{
			break;
		}
		else
		{
			%tgts = restWords(%tgts);
		}
	}
	if(isObject(%tgt))
	{
		//warning: math
		%tgtPos = %tgt.getHackPosition();
		%dist = vectorDist(%start, %tgtPos);
		%time = %dist / %vel;
		%tgtPos = vectorAdd(%tgtPos, vectorScale(%tgt.getVelocity(), %time));
		%dropCorrection = 9.81 * %projData.gravityMod * mPow(%time, 2);
		%vec = vectorScale(vectorNormalize(vectorSub(%tgtPos, %start)), %vel);
		%vec = vectorAdd(%vec, "0 0 " @ %dropCorrection);
	}
	else
	{
		%spread = vectorNormalize((getRandom() - 0.5) SPC (getRandom() - 0.5) SPC (getRandom() - 0.5));
		%vec = vectorScale(%obj.getEyeVector(), 10);
		%vec = vectorNormalize(vectorAdd(%vec, %spread));
		%vec = vectorScale(%vec, %vel);
	}
	%proj = new Projectile()
	{
		datablock = %projData;
		initialPosition = %start;
		initialVelocity = %vec;
		sourceObject = %obj;
		sourceSlot = %slot;
		damage = %proj.damage / 2;
		scale = "0.7 0.7 0.7";
		headshot = true;
	};
}

datablock ItemData(CrystalBowItem : WoodenBowItem)
{
	image = CrystalBowImage;
	colorShiftColor = "0.92 0.55 1 0.78";
	uiName = "Crystal Bow";
};

datablock ShapeBaseImageData(CrystalBowImage : WoodenBowImage)
{
	item = CrystalBowItem;
	colorShiftColor = CrystalBowItem.colorShiftColor;

	stateTimeoutValue[3] = 0.35;
};

function CrystalBowImage::onFire(%this, %obj, %slot)
{
	//projectile info
	%proj = DoBowShot(%obj, %slot, true);
	if(!isObject(%proj))
	{
		return;
	}
	%projData = %proj.getDatablock();
	%vel = vectorLen(%proj.getVelocity());
	//target info
	%start = %obj.getMuzzlePoint(%slot);
	%masks = $Typemasks::PlayerObjectType;
	%tgts = conalRaycastM(%start, %obj.getEyeVector(), 300, 7.5, %masks, %obj);
	while((%tgt = firstWord(%tgts)) !$= "")
	{
		if(minigameCanDamage(%obj, %tgt))
		{
			break;
		}
		else
		{
			%tgts = restWords(%tgts);
		}
	}
	if(isObject(%tgt))
	{
		//warning: math
		%tgtPos = %tgt.getHackPosition();
		%dist = vectorDist(%start, %tgtPos);
		%time = %dist / %vel;
		%tgtPos = vectorAdd(%tgtPos, vectorScale(%tgt.getVelocity(), %time));
		%dropCorrection = 9.81 * %projData.gravityMod * mPow(%time, 2);
		%vec = vectorScale(vectorNormalize(vectorSub(%tgtPos, %start)), %vel);
		%vec = vectorAdd(%vec, "0 0 " @ %dropCorrection);
	}
	else
	{
		%spread = vectorNormalize((getRandom() - 0.5) SPC (getRandom() - 0.5) SPC (getRandom() - 0.5));
		%vec = vectorScale(%obj.getEyeVector(), 10);
		%vec = vectorNormalize(vectorAdd(%vec, %spread));
		%vec = vectorScale(%vec, %vel);
	}
	%proj = new Projectile()
	{
		datablock = %projData;
		initialPosition = %start;
		initialVelocity = %vec;
		sourceObject = %obj;
		sourceSlot = %slot;
		damage = %proj.damage;
		headshot = true;
	};
}

// #3.2

datablock ParticleData(snowballExplosionParticle)
{
	dragCoefficient = 4.5;
	gravityCoefficient = 2.5;
	inheritedVelFactor = 0.4;
	constantAcceleration = 0;
	lifetimeMS = 400;
	lifetimeVarianceMS = 300;
	textureName = "base/data/particles/dot";
	spinSpeed = 0;
	spinRandomMin = 0;
	spinRandomMax = 0;
	colors[0] = "1 1 1 0.9";
	colors[1] = "1 1 1 0.0";
	sizes[0] = 0.45;
	sizes[1] = 0.0;

	useInvAlpha = true;
};
datablock ParticleEmitterData(snowballExplosionEmitter)
{
	ejectionPeriodMS = 3;
	periodVarianceMS = 0;
	ejectionVelocity = 15;
	velocityVariance = 7.5;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = false;
	particles = "snowballExplosionParticle";

	useEmitterColors = true;
	uiName = "Snowball Explosion";
};

datablock ExplosionData(snowballExplosion)
{
	soundProfile = "";

	lifeTimeMS = 50;

	emitter[0] = snowballExplosionEmitter;

	faceViewer     = true;
	explosionScale = "1 1 1";

	shakeCamera = false;
	camShakeFreq = "10.0 11.0 10.0";
	camShakeAmp = "1.0 1.0 1.0";
	camShakeDuration = 0.5;
	camShakeRadius = 10.0;

	lightStartRadius = 0;
	lightEndRadius = 0;
	lightStartColor = "0 0 0";
	lightEndColor = "0 0 0";
};

datablock ProjectileData(snowballProjectile)
{
	projectileShapeName = "base/data/shapes/snowball.dts";

	explosion = snowballExplosion;

	armingDelay = 0;
	lifetime = 30000;
	fadeDelay = 30000;

	isBallistic = true;
	bounceAngle = 170;
	minStickVelocity = 10;
	bounceElasticity = 0.5;
	bounceFriction = 0;
	gravityMod = 1;

	hasLight = false;

	muzzleVelocity = 30;
	velInheritFactor = 1;

	uiName = "Snowball";
};

function snowballProjectile::onCollision(%this, %obj, %col, %fade, %pos, %normal)
{
	serverPlay3d("sandHitSound" @ getRandom(1, 3), %pos);
}

datablock ItemData(snowballItem)
{
	category = "Weapon";
	className = "Weapon";

	shapeFile = "base/data/shapes/snowball.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	uiName = "Snowball";
	iconName = "Add-Ons/Gamemode_ReverieFortWars/models/icon_Snowball";
	doColorShift = true;
	colorShiftColor = "1 1 1 1";

	image = snowballImage;
	canDrop = true;
};

datablock ShapeBaseImageData(snowballImage)
{
	shapeFile = "base/data/shapes/snowball.dts";
	emap = true;

	mountPoint = 0;
	offset = "0 0 0";

	correctMuzzleVector = true;

	className = "WeaponImage";

	item = snowballItem;
	ammo = " ";
	projectile = snowballProjectile;
	projectileType = Projectile;

	melee = false;
	armReady = true;

	doColorShift = true;
	colorShiftColor = "1 1 1 1";

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.1;
	stateTransitionOnTimeout[0] = "Ready";
	stateSequence[0] = "ready";

	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "Charge";
	stateAllowImageChange[1] = true;

	stateName[2] = "Charge";
	stateTransitionOnTimeout[2] = "Armed";
	stateTimeoutValue[2] = 0.7;
	stateWaitForTimeout[2] = false;
	stateTransitionOnTriggerUp[2] = "AbortCharge";
	stateScript[2] = "onCharge";
	stateAllowImageChange[2] = false;

	stateName[3] = "AbortCharge";
	stateTransitionOnTimeout[3] = "Ready";
	stateTimeoutValue[3] = 0.3;
	stateWaitForTimeout[3] = true;
	stateScript[3] = "onAbortCharge";
	stateAllowImageChange[3] = false;

	stateName[4] = "Armed";
	stateTransitionOnTriggerUp[4] = "Fire";
	stateAllowImageChange[4] = false;

	stateName[5] = "Fire";
	stateTransitionOnTimeout[5] = "Ready";
	stateTimeoutValue[5] = 0.5;
	stateFire[5] = true;
	stateScript[5] = "onFire";
	stateWaitForTimeout[5] = true;
	stateAllowImageChange[5]  = false;
};

function snowballImage::onMount(%this, %obj, %slot)
{
	%obj.playAudio(%slot, "sandHitSound" @ getRandom(1, 3));
	parent::onMount(%this, %obj, %slot);
}

function snowballImage::onCharge(%this, %obj, %slot)
{
	%obj.playthread(2, spearReady);
}

function snowballImage::onAbortCharge(%this, %obj, %slot)
{
	%obj.playthread(2, root);
}

function snowballImage::onFire(%this, %obj, %slot)
{
	%obj.playthread(2, spearThrow);
	Parent::onFire(%this, %obj, %slot);
	%acc = %obj.client.account;
	if(!isObject(%acc))
	{
		return;
	}
	%item = %acc.read("Equip" @ %obj.currtool);
	%q = getItemQuantity(%item) - 1;
	%obj.updateBottomPrintHUD("Bow", "Snowball" TAB %q);
	if(%q < 1)
	{
		%acc.set("Equip" @ %obj.currtool, "");
		%obj.unmountImage(%slot);
		%obj.playThread(1, "root");
		%obj.tool[%obj.currTool] = "";
		messageClient(%obj.client, 'MsgItemPickup', '', %obj.currTool, "");
	}
	else
	{
		%acc.set("Equip" @ %obj.currtool, setItemQuantity(%item, %q));
	}
}

// #4.

function Player::SubtractMana(%obj, %amt)
{
	%reduction = %obj.manaReduction != 0 ? %obj.manaReduction : 1;
	%mana = %obj.getEnergyLevel() - %amt * %reduction;
	if(%mana > 0)
	{
		%obj.setEnergyLevel(%mana);
		return true;
	}
	else
	{
		return false;
	}
}

// #4.1

datablock AudioProfile(FireLoopSound)
{
	filename = "Add-Ons/Gamemode_ReverieFortWars/sounds/fire_loop.wav";
	description = AudioCloseLooping3d;
	preload = true;
};

datablock AudioProfile(FaeFireImpactSound)
{
	filename = "Add-Ons/Gamemode_ReverieFortWars/sounds/faefire_impact.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock ParticleData(FaeFireTrailParticle)
{
   dragCoefficient = 1;
   windCoefficient = 0;
   gravityCoefficient = -0.5;
   inheritedVelFactor = 0.2;
   constantAcceleration = 0;
   lifetimeMS = 300;
   lifetimeVarianceMS = 100;
   spinSpeed = 0;
   spinRandomMin = -900;
   spinRandomMax = 900;
   useInvAlpha = false;
   animateTexture = false;
   textureName = "base/data/particles/cloud";
 
   colors[0] = "0.2 1   0.4 0.1";
   colors[1] = "0.2 0.8 0.4 0.5";
   colors[2] = "1   0   0.5 0.1";
   sizes[0] = 0.2;
   sizes[1] = 0.6;
   sizes[2] = 0.2;
   times[0] = 0;
   times[1] = 0.2;
   times[2] = 1;
};

datablock ParticleEmitterData(FaeFireTrailEmitter)
{
	ejectionPeriodMS = 3;
	periodVarianceMS = 0;
	ejectionVelocity = 1.5;
	velocityVariance = 0.5;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 160;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	orientParticles = 0;
	orientOnVelocity = 1;
	particles = "FaeFireTrailParticle";
	useEmitterSizes = false;
	useEmitterColors = false;
	uiName = "Faerie Fire Trail";
};

datablock ParticleData(FaeFireExplosionParticle)
{
	dragCoefficient = 1;
	windCoefficient = 0;
	gravityCoefficient = -0.5;
	inheritedVelFactor = 0.2;
	constantAcceleration = 0;
	lifetimeMS = 750;
	lifetimeVarianceMS = 250;
	spinSpeed = 0;
	spinRandomMin = -180;
	spinRandomMax = 180;
	useInvAlpha = false;
	textureName = "base/data/particles/cloud";
	colors[0] = "0.2 1   0.4 0.1";
	colors[1] = "0.2 0.8 0.4 0.5";
	colors[2] = "1   0   0.5 0.1";
	sizes[0] = 0.5;
	sizes[1] = 1.2;
	sizes[2] = 0.2;
	times[0] = 0;
	times[1] = 0.2;
	times[2] = 1;
};

datablock ParticleEmitterData(FaeFireExplosionEmitter)
{
	ejectionPeriodMS = 5;
	periodVarianceMS = 0;
	ejectionVelocity = 2.5;
	velocityVariance = 1;
	ejectionOffset = 0.2;
	thetaMin = 0;
	thetaMax = 160;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	orientParticles = 0;
	orientOnVelocity = 1;
	particles = "FaeFireExplosionParticle";
	lifetimeMS = 0;
	lifetimeVarianceMS = 0;
	useEmitterSizes = 0;
	useEmitterColors = 0;
	uiName = "Faerie Fire Explosion";
};

datablock ExplosionData(FaeFireExplosion)
{
	soundProfile = FaeFireImpactSound;

	lifeTimeMS = 800;

	emitter[0] = FaeFireExplosionEmitter;

	faceViewer     = true;
	explosionScale = "1 1 1";

	shakeCamera = false;
	camShakeFreq = "10.0 11.0 10.0";
	camShakeAmp = "1.0 1.0 1.0";
	camShakeDuration = 0.5;
	camShakeRadius = 10;

	lightStartRadius = 8;
	lightEndRadius = 0;
	lightStartColor = "0.2 1 0.4";
	lightEndColor = "1 0 0.5";

	damageRadius = 2;
	radiusDamage = 30;
	playerBurnTime = 5000;
};

datablock ProjectileData(FaeFireProjectile)
{
	projectileShapeName = "base/data/shapes/empty.dts";

	explosion = FaeFireExplosion;

	directDamage = 10;
	directDamageType = $DamageType::FaeFire;
	radiusDamageType = $DamageType::FaeFire;

	armingDelay = 0;
	lifetime = 4000;
	fadeDelay = 4000;

	isBallistic = true;
	ballRadius = 1;
	bounceAngle = 170;
	minStickVelocity = 10;
	bounceElasticity = 0.5;
	bounceFriction = 0;
	gravityMod = 1;

	hasLight = true;
	lightRadius = 10;
	lightColor = "0.2 1 0.4";

	muzzleVelocity = 80;
	velInheritFactor = 0;

	sound = FaeFireImpactSound;
	particleEmitter = FaeFireTrailEmitter;

	uiName = "Faerie Fire";
};

function faeFireProjectile::onCollision(%this, %obj, %col, %fade, %pos, %normal)
{
	if(%col.getClassName() $= "fxDtsBrick" && (!isObject(%col.emitter) || isEventPending(%col.unburnSched)))
	{
		if(%col.getVolume() < 10)
		{
			cancel(%col.emberSched);
			cancel(%col.unburnSched);
			%col.setEmitter(FaeFireExplosionEmitter);
			%col.emberSched = %col.schedule(5000, setEmitter, FaeFireTrailemitter);
			%col.unburnSched = %col.schedule(15000, setEmitter);
		}
	}
	else
	{
		return parent::onCollision(%this, %obj, %col, %fade, %pos, %normal);
	}
}

datablock ShapeBaseImageData(faeFireImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = true;

	mountPoint = $HeadSlot;
	offset = "0 1 0";

	correctMuzzleVector = false;

	className = "WeaponImage";

	item = " ";
	ammo = " ";
	projectile = FaeFireProjectile;
	projectileType = Projectile;

	melee = false;
	armReady = true;

	doColorShift = false;
	colorShiftColor = "1 1 1 1";

	stateName[0] = "Ready";
	stateTransitionOnTriggerDown[0] = "Fire";

	stateName[1] = "Fire";
	stateTransitionOnTriggerUp[1] = "Ready";
	stateFire[1] = true;
	stateScript[1] = "onFire";
};

// #4.2

datablock ItemData(FaerieWandItem)
{
	category = "Weapon";
	className = "Weapon";

	shapeFile = "base/data/shapes/wand.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	uiName = "Faerie wand";
	iconName = "base/client/ui/itemIcons/wand";
	doColorShift = true;
	colorShiftColor = "0 0.7 0.3 1";

	image = FaerieWandImage;
	canDrop = true;
};

datablock ShapeBaseImageData(FaerieWandImage)
{
	shapeFile = FaerieWandItem.shapeFile;
	emap = true;

	mountPoint = 0;
	offset = "0 0 0";

	correctMuzzleVector = false;

	eyeOffset = "0 0 0";

	className = "WeaponImage";

	item = FaerieWandItem;
	ammo = " ";

	melee = true;
	doRetraction = false;
	armReady = true;

	doColorShift = true;
	colorShiftColor = FaerieWandItem.colorShiftColor;

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.5;
	stateTransitionOnTimeout[0] = "Ready";
	stateSound[0] = weaponSwitchSound;

	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "PreFire";
	stateAllowImageChange[1] = true;

	stateName[2] = "PreFire";
	stateScript[2] = "onPreFire";
	stateAllowImageChange[2] = false;
	stateTimeoutValue[2] = 0.1;
	stateTransitionOnTimeout[2] = "Fire";

	stateName[3] = "Fire";
	stateTransitionOnTimeout[3] = "CheckFire";
	stateTimeoutValue[3]= 0.2;
	stateAllowImageChange[3] = false;
	stateEmitter[3] = FaeFireTrailEmitter;
	stateEmitterTime[3] = 0.2;
	stateEmitterNode[3] = muzzlePoint;
	stateScript[3] = "onFire";
	stateWaitForTimeout[3] = true;

	stateName[4] = "CheckFire";
	stateTransitionOnTriggerUp[4]	= "StopFire";
	stateTransitionOnTriggerDown[4] = "Fire";

	stateName[5] = "StopFire";
	stateTransitionOnTimeout[5] = "Ready";
	stateTimeoutValue[5] = 0.2;
	stateAllowImageChange[5] = false;
	stateWaitForTimeout[5] = true;
	stateSequence[5] = "StopFire";
	stateScript[5] = "onStopFire";
};

function FaerieWandImage::onPreFire(%this, %obj, %slot)
{
	%obj.playthread(2, armattack);
}

function FaerieWandImage::onStopFire(%this, %obj, %slot)
{	
	%obj.playthread(2, root);
}

function FaerieWandImage::onFire(%this, %obj, %slot)
{
	%start = %obj.getEyePoint();
	%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), 10));
	%ray = containerRaycast(%start, %end, $Typemasks::FxBrickAlwaysObjectType);
	if(isObject(%col = firstWord(%ray)))
	{
		if(getTrustLevel(%obj, %col) >= 2)
		{
			%pos = posFromRaycast(%ray);
			%norm = normalFromRaycast(%ray);
			%p = new projectile()
			{
				datablock = faeFireProjectile;
				initialPosition = %pos;
				initialVelocity = %norm;
				scale = "0.5 0.5 0.5";
			};
			%p.explode();
			if((%fx = %obj.client.currentFxCan) != 0)
			{
				if(%fx == 7 || %fx == 8)
				{
					%col.setShapeFx(%fx - 7);
				}
				else
				{
					%col.setColorFx(%fx);
				}
			}
			else
			{
				WrenchImage::onHitObject(%this, %obj, %slot, %col, %pos, %norm);
			}
		}
		else if(%ray.getGroup().bl_id == 888888 && %col.getDatablock() == nameToID(brick16x16fData))
		{
			if(!%obj.subtractMana(5))
			{
				return;
			}
			%pos = posFromRaycast(%ray);
			%norm = normalFromRaycast(%ray);
			%p = new projectile()
			{
				datablock = faeFireProjectile;
				initialPosition = %pos;
				initialVelocity = %norm;
				scale = "0.5 0.5 0.5";
			};
			%p.explode();
			%brick = new fxDtsBrick()
			{
				position = vectorAdd(%pos, "0 0 2.3");
				datablock = brickPineTreeData;
				colorID = $RFW::ResourceColor["Wood"];
				client = $PublicClient;
				stackBL_ID = 888888;
				isPlanted = true;
			};
			%err = SAG_BrickPlantStuff(%brick, $PublicClient);
			if(%err == 0)
			{
				%obj.setEnergyLevel(%obj.getEnergyLevel() - 5);
				%brick.setEmitter(FaeFireExplosionEmitter);
				%brick.schedule(200, setEmitter);
			}
			else
			{
				%brick.delete();
			}
		}
	}
}

package RFW_FaerieWand
{
	function fxDtsBrick::setItem(%this, %item, %client)
	{
		if(isObject(%item) && isObject(%client) && !%client.isAdmin)
		{
			messageClient(%client, 'MsgError');
			%client.centerPrintQueue("<color:FF0000>Cannot spawn items.", 3000);
			return;
		}
		return parent::setItem(%this, %item, %client);
	}
	function serverCmdAddEvent(%client, %a, %b, %c, %d, %e, %f, %g, %h, %i, %j)
	{
		if(isObject(%client) && !%client.isAdmin)
		{
			messageClient(%client, 'MsgError');
			%client.centerPrintQueue("<color:FF0000>Cannot add events.", 3000);
			return;
		}
		parent::serverCmdAddEvent(%client, %a, %b, %c, %d, %e, %f, %g, %h, %i, %j);
	}
};
activatePackage(RFW_FaerieWand);

// #4.3

datablock AudioProfile(photoLensSound1)
{
	filename = "Add-Ons/Gamemode_ReverieFortWars/sounds/photoLens1.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(photoLensSound2)
{
	filename = "Add-Ons/Gamemode_ReverieFortWars/sounds/photoLens2.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(photoLensSound3)
{
	filename = "Add-Ons/Gamemode_ReverieFortWars/sounds/photoLens3.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(photoLensProjectileSound)
{
	filename = "Add-Ons/Gamemode_ReverieFortWars/sounds/sparkler_loop.wav";
	description = AudioCloseLooping3d;
	preload = true;
};

datablock ParticleData(PhotoLensTrailParticle)
{
	dragCoefficient = 0;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 5;
	lifetimeMS = 100;
	lifetimeVarianceMS = 5;
	spinSpeed = 0;
	spinRandomMin = -900;
	spinRandomMax = 900;
	useInvAlpha = false;
	textureName = "base/data/particles/cloud";
	colors[0] = "1 0 0 1";
	colors[1] = "0 1 0 1";
	colors[2] = "0 0 1 1";
	colors[3] = "1 0 1 0";
	sizes[0] = "0.3";
	sizes[1] = "0.3";
	sizes[2] = "0.3";
	sizes[3] = "0.3";
	times[0] = 0;
	times[1] = 0.5;
	times[2] = 0.75;
	times[3] = 1;
};

datablock ParticleEmitterData(PhotoLensTrailEmitter)
{
	className = "ParticleEmitterData";
	ejectionPeriodMS = 5;
	periodVarianceMS = 0;
	ejectionVelocity = 0;
	velocityVariance = 0;
	ejectionOffset = 0.5;
	thetaMin = 90;
	thetaMax = 90;
	phiReferenceVel = 2400;
	phiVariance = 0;
	overrideAdvance = 0;
	orientParticles = 0;
	orientOnVelocity = 1;
	particles = "PhotoLensTrailParticle";
	lifetimeMS = 0;
	lifetimeVarianceMS = 0;
	useEmitterSizes = 0;
	useEmitterColors = 0;
	uiName = "Faerie Fire Trail";
};

datablock ParticleData(PhotoLensExplosionParticle)
{
	dragCoefficient = 0;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 5;
	lifetimeMS = 420;
	lifetimeVarianceMS = 0;
	spinSpeed = 0;
	spinRandomMin = -900;
	spinRandomMax = 900;
	useInvAlpha = false;
	textureName = "base/data/particles/cloud";
	colors[0] = "1 0 1 0";
	colors[1] = "0.1 0 1 1";
	colors[2] = "0.2 1 0 1";
	colors[3] = "1 0 0 0.5";
	sizes[0] = "0.3";
	sizes[1] = "0.3";
	sizes[2] = "0.4";
	sizes[3] = 1;
	times[0] = 0;
	times[1] = 0.25;
	times[2] = 0.5;
	times[3] = 1;
};

datablock ParticleEmitterData(PhotoLensExplosionEmitter)
{
	className = "ParticleEmitterData";
	ejectionPeriodMS = 2;
	periodVarianceMS = 0;
	ejectionVelocity = -5;
	velocityVariance = 0;
	ejectionOffset = 5;
	thetaMin = 0;
	thetaMax = 45;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	orientParticles = 0;
	orientOnVelocity = 1;
	particles = "PhotoLensExplosionParticle";
	lifetimeMS = 0;
	lifetimeVarianceMS = 0;
	useEmitterSizes = 0;
	useEmitterColors = 0;
	uiName = "PhotoMS Lens Explosion";
};

datablock ExplosionData(PhotoLensExplosion)
{
	soundProfile = "PhotoLensImpactSound";
	particleEmitter = "PhotoLensTrailEmitter";
	particleDensity = 25;
	particleRadius = 0.25;
	emitter[0] = "PhotoLensExplosionEmitter";
	emitter[1] = "PhotoLensTrailEmitter";
	lifetimeMS = 350;
	shakeCamera = false;
	lightStartRadius = 10;
	lightEndRadius = 0;
	lightStartColor = "1 1 0.5 1";
	lightEndColor = "0.5 0.5 1 1";
	damageRadius = 2;
	radiusDamage = 30;
};

datablock ProjectileData(PhotoLensProjectile)
{
	projectileShapeName = "base/data/shapes/empty.dts";
	particleEmitter = PhotoLensTrailEmitter;
	sound = photoLensProjectileSound;
	Explosion = PhotoLensExplosion;
	hasLight = true;
	lightRadius = 10;
	lightColor = "1 1 1";
	isBallistic = 1;
	velInheritFactor = 0;
	muzzleVelocity = 80;
	lifetime = 5000;
	armingDelay = 0;
	fadeDelay = 125;
	bounceAngle = 170;
	minStickVelocity = 10;
	ballRadius = 0.7;
	uiName = "PhotoMS Lens";
	bounceElasticity = 0.5;
	bounceFriction = 0;
	gravityMod = 0;
};

function PhotoLensProjectile::onCollision(%this, %obj, %col, %fade, %pos, %normal)
{
	if(%col.getType() & $Typemasks::PlayerObjectType && minigameCanDamage(%obj.sourceObject, %col))
	{
		%col.damage(%obj.sourceObject, %pos, 20, $DamageType::Lens);
	}
	else if(%col.getType() & $Typemasks::FxBrickObjectType && (getTrustLevel(%obj.sourceObject, %col) >= 2 || %col.getGroup().bl_id == 888888))
	{
		if(%col.getVolume() <= 4)
		{
			if(getWord(%col.getPosition(), 2) >= 312)
			{
				%col.setColor($RFW::ResourceColor["Sinnite"]);
				%col.setColorFx($RFW::ResourceColorFx["Sinnite"]);
			}
			else
			{
				%col.setColor($RFW::ResourceColor["Nithlite"]);
				%col.setColorFx($RFW::ResourceColorFx["Nithlite"]);
			}
		}
	}
}

datablock ItemData(PhotoLensItem)
{
	category = "Weapon";
	className = "Weapon";

	shapeFile = "Add-Ons/GameMode_ReverieFortWars/models/Photometallisynthesis_Lens.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	uiName = "PhotoMS lens";
	iconName = "Add-Ons/GameMode_ReverieFortWars/models/PhotoLens";
	doColorShift = false;
	colorShiftColor = "1 1 1 1";

	image = PhotoLensImage;
	canDrop = true;
};

datablock ShapeBaseImageData(PhotoLensImage)
{
	shapeFile = PhotoLensItem.shapeFile;
	emap = true;

	mountPoint = 0;
	offset = "-0.5 0 0";

	correctMuzzleVector = false;

	eyeOffset = "0 0 0";

	className = "WeaponImage";

	item = PhotoLensItem;
	ammo = " ";
	projectile = PhotoLensProjectile;

	melee = true;
	doRetraction = false;
	armReady = true;

	doColorShift = PhotoLensItem.doColorShift;
	colorShiftColor = PhotoLensItem.colorShiftColor;

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.5;
	stateTransitionOnTimeout[0] = "Ready";
	stateSound[0] = weaponSwitchSound;

	stateName[1] = "Ready";
	stateTimeoutValue[1] = 5;
	stateWaitForTimeout[1] = false;
	stateTransitionOnTimeout[1] = "Presynthesis";
	stateTransitionOnTriggerDown[1] = "Fire";
	stateAllowImageChange[1] = true;

	stateName[2] = "Presynthesis";
	stateScript[2] = "Presynthesis";
	stateAllowImageChange[2] = false;
	stateTimeoutValue[2] = 1.5;
	stateWaitForTimeout[2] = true;
	stateTransitionOnTimeout[2] = "Synthesis";
	stateEmitter[2] = PhotoLensTrailEmitter;
	stateEmitterNode[2] = muzzlePoint;
	stateEmitterTime[2] = 1.5;

	stateName[3] = "Synthesis";
	stateScript[3] = "Synthesis";
	stateAllowImageChange[3] = false;
	stateTimeoutValue[3] = 0.5;
	stateWaitForTimeout[3] = false;
	stateTransitionOnTimeout[3] = "Ready";
	stateTransitionOnTriggerDown[3] = "Fire";
	stateEmitter[3] = PhotoLensExplosionEmitter;
	stateEmitterNode[3] = muzzlePoint;
	stateEmitterTime[3] = 0.1;

	stateName[4] = "Fire";
	stateFire[4] = true;
	stateScript[4] = "onFire";
	stateAllowImageChange[4] = false;
	stateTimeoutValue[4] = 1;
	stateWaitForTimeout[4] = true;
	stateTransitionOnTimeout[4] = "AltReady";

	stateName[5] = "AltReady";
	stateTransitionOnTriggerDown[5] = "Fire";
	stateAllowImageChange[5] = true;
};

function PhotoLensImage::onMount(%this, %obj, %slot)
{
	parent::onMount(%this, %obj, %slot);
	%obj.playThread(1, armreadyboth);
}

function PhotoLensImage::Presynthesis(%this, %obj, %slot)
{
	serverPlay3d("PhotoLensSound" @ getRandom(1, 3), %obj.getMuzzlePoint(%slot));
}

function PhotoLensImage::Synthesis(%this, %obj, %slot)
{
	%mat = getWord(%obj.getPosition(), 2) >= 308 ? "Sinnite" : "Nithlite";
	if(%obj.subtractMana(16))
	{
		%obj.client.account.addItem(%mat TAB 8, true);
	}
	else
	{
		%energy = %obj.getEnergyLevel();
		%amt = mFloatLength(0.5 * %energy - 0.05, 1);
		%obj.setEnergyLevel(0);
		%obj.client.account.addItem(%mat TAB %amt, true);
	}
}

function PhotoLensImage::onFire(%this, %obj, %slot)
{
	if(%obj.subtractMana(4))
	{
		%proj = new Projectile()
		{
			datablock = PhotoLensProjectile;
			initialPosition = %obj.getMuzzlePoint(%slot);
			initialVelocity = vectorScale(%obj.getMuzzleVector(%slot), 100);
			sourceObject = %obj;
			damage = 30;
		};
	}
}

// #4.4
datablock ParticleData(PyroWandTrailParticle)
{
	dragCoefficient = 0;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 5;
	lifetimeMS = 100;
	lifetimeVarianceMS = 5;
	spinSpeed = 0;
	spinRandomMin = -900;
	spinRandomMax = 900;
	useInvAlpha = false;
	textureName = "base/data/particles/cloud";
	colors[0] = "1 0 0 1";
	colors[1] = "0 1 0 1";
	colors[2] = "0 0 1 1";
	colors[3] = "1 0 1 0";
	sizes[0] = "0.3";
	sizes[1] = "0.3";
	sizes[2] = "0.3";
	sizes[3] = "0.3";
	times[0] = 0;
	times[1] = 0.5;
	times[2] = 0.75;
	times[3] = 1;
};

datablock ParticleEmitterData(PyroWandTrailEmitter)
{
	className = "ParticleEmitterData";
	ejectionPeriodMS = 5;
	periodVarianceMS = 0;
	ejectionVelocity = 0;
	velocityVariance = 0;
	ejectionOffset = 0.5;
	thetaMin = 90;
	thetaMax = 90;
	phiReferenceVel = 2400;
	phiVariance = 0;
	overrideAdvance = 0;
	orientParticles = 0;
	orientOnVelocity = 1;
	particles = "PyroWandTrailParticle";
	lifetimeMS = 0;
	lifetimeVarianceMS = 0;
	useEmitterSizes = 0;
	useEmitterColors = 0;
	uiName = "Pyro Wand Trail";
};

datablock ParticleData(PyroWandExplosionParticle)
{
	dragCoefficient = 0;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 5;
	lifetimeMS = 420;
	lifetimeVarianceMS = 0;
	spinSpeed = 0;
	spinRandomMin = -900;
	spinRandomMax = 900;
	useInvAlpha = false;
	textureName = "base/data/particles/cloud";
	colors[0] = "1 0 1 0";
	colors[1] = "0.1 0 1 1";
	colors[2] = "0.2 1 0 1";
	colors[3] = "1 0 0 0.5";
	sizes[0] = "0.3";
	sizes[1] = "0.3";
	sizes[2] = "0.4";
	sizes[3] = 1;
	times[0] = 0;
	times[1] = 0.25;
	times[2] = 0.5;
	times[3] = 1;
};

datablock ParticleEmitterData(PyroWandExplosionEmitter)
{
	className = "ParticleEmitterData";
	ejectionPeriodMS = 2;
	periodVarianceMS = 0;
	ejectionVelocity = -5;
	velocityVariance = 0;
	ejectionOffset = 5;
	thetaMin = 0;
	thetaMax = 45;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	orientParticles = 0;
	orientOnVelocity = 1;
	particles = "PyroWandExplosionParticle";
	lifetimeMS = 0;
	lifetimeVarianceMS = 0;
	useEmitterSizes = 0;
	useEmitterColors = 0;
	uiName = "Pyro Wand Explosion";
};

datablock ExplosionData(PyroWandExplosion)
{
	soundProfile = "PyroWandImpactSound";
	particleEmitter = "PyroWandTrailEmitter";
	particleDensity = 25;
	particleRadius = 0.25;
	emitter[0] = "PyroWandExplosionEmitter";
	emitter[1] = "PyroWandTrailEmitter";
	lifetimeMS = 350;
	shakeCamera = false;
	lightStartRadius = 10;
	lightEndRadius = 0;
	lightStartColor = "1 1 0.5 1";
	lightEndColor = "0.5 0.5 1 1";
	damageRadius = 2;
	radiusDamage = 30;
};

datablock ProjectileData(PyroWandProjectile)
{
	projectileShapeName = "base/data/shapes/empty.dts";
	particleEmitter = PyroWandTrailEmitter;
	sound = FireLoopSound;
	Explosion = PyroWandExplosion;
	hasLight = true;
	lightRadius = 10;
	lightColor = "1 1 1";
	isBallistic = 1;
	velInheritFactor = 0;
	muzzleVelocity = 80;
	lifetime = 5000;
	armingDelay = 0;
	fadeDelay = 125;
	bounceAngle = 170;
	minStickVelocity = 10;
	ballRadius = 0.7;
	uiName = "Pyro wand";
	bounceElasticity = 0.5;
	bounceFriction = 0;
	gravityMod = 0;
};

datablock ItemData(PyroWandItem)
{
	category = "Weapon";
	className = "Weapon";

	shapeFile = "Add-Ons/GameMode_ReverieFortWars/models/Pyromancy_Wand.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	uiName = "Pyro wand";
	iconName = "Add-Ons/GameMode_ReverieFortWars/models/PyroWand";
	doColorShift = false;
	colorShiftColor = "1 1 1 1";

	image = PyroWandImage;
	canDrop = true;
};

datablock ShapeBaseImageData(PyroWandImage)
{
	shapeFile = PyroWandItem.shapeFile;
	emap = true;

	mountPoint = 0;
	offset = "0 0 0";

	correctMuzzleVector = false;

	eyeOffset = "0 0 0";

	className = "WeaponImage";

	item = PyroWandItem;
	ammo = " ";
	projectile = PyroWandProjectile;
	projectileType = projectile;

	melee = true;
	doRetraction = false;
	armReady = true;

	doColorShift = PyroWandItem.doColorShift;
	colorShiftColor = PyroWandItem.colorShiftColor;

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.5;
	stateTransitionOnTimeout[0] = "Ready";
	stateSound[0] = weaponSwitchSound;

	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "Fire";
	stateAllowImageChange[1] = true;

	stateName[2] = "Fire";
	stateFire[2] = true;
	stateScript[2] = "onFire";
	stateAllowImageChange[2] = false;
	stateTimeoutValue[2] = 1;
	stateWaitForTimeout[2] = true;
	stateTransitionOnTimeout[2] = "Ready";
};

// #4.5
datablock AudioProfile(EvilLoopSound)
{
	filename = "Add-Ons/Gamemode_ReverieFortWars/sounds/evil_loop.wav";
	description = AudioCloseLooping3d;
	preload = true;
};

datablock ParticleData(OssiStaffTrailParticle)
{
	dragCoefficient = 0;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 5;
	lifetimeMS = 100;
	lifetimeVarianceMS = 5;
	spinSpeed = 0;
	spinRandomMin = -900;
	spinRandomMax = 900;
	useInvAlpha = false;
	textureName = "base/data/particles/cloud";
	colors[0] = "1 0 0 1";
	colors[1] = "0 1 0 1";
	colors[2] = "0 0 1 1";
	colors[3] = "1 0 1 0";
	sizes[0] = "0.3";
	sizes[1] = "0.3";
	sizes[2] = "0.3";
	sizes[3] = "0.3";
	times[0] = 0;
	times[1] = 0.5;
	times[2] = 0.75;
	times[3] = 1;
};

datablock ParticleEmitterData(OssiStaffTrailEmitter)
{
	className = "ParticleEmitterData";
	ejectionPeriodMS = 5;
	periodVarianceMS = 0;
	ejectionVelocity = 0;
	velocityVariance = 0;
	ejectionOffset = 0.5;
	thetaMin = 90;
	thetaMax = 90;
	phiReferenceVel = 2400;
	phiVariance = 0;
	overrideAdvance = 0;
	orientParticles = 0;
	orientOnVelocity = 1;
	particles = "OssiStaffTrailParticle";
	lifetimeMS = 0;
	lifetimeVarianceMS = 0;
	useEmitterSizes = 0;
	useEmitterColors = 0;
	uiName = "OssiMS Staff Trail";
};

datablock ParticleData(OssiStaffExplosionParticle)
{
	dragCoefficient = 0;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 5;
	lifetimeMS = 420;
	lifetimeVarianceMS = 0;
	spinSpeed = 0;
	spinRandomMin = -900;
	spinRandomMax = 900;
	useInvAlpha = false;
	textureName = "base/data/particles/cloud";
	colors[0] = "1 0 1 0";
	colors[1] = "0.1 0 1 1";
	colors[2] = "0.2 1 0 1";
	colors[3] = "1 0 0 0.5";
	sizes[0] = "0.3";
	sizes[1] = "0.3";
	sizes[2] = "0.4";
	sizes[3] = 1;
	times[0] = 0;
	times[1] = 0.25;
	times[2] = 0.5;
	times[3] = 1;
};

datablock ParticleEmitterData(OssiStaffExplosionEmitter)
{
	className = "ParticleEmitterData";
	ejectionPeriodMS = 2;
	periodVarianceMS = 0;
	ejectionVelocity = -5;
	velocityVariance = 0;
	ejectionOffset = 5;
	thetaMin = 0;
	thetaMax = 45;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	orientParticles = 0;
	orientOnVelocity = 1;
	particles = "OssiStaffExplosionParticle";
	lifetimeMS = 0;
	lifetimeVarianceMS = 0;
	useEmitterSizes = 0;
	useEmitterColors = 0;
	uiName = "OssiMS Staff Explosion";
};

datablock ExplosionData(OssiStaffExplosion)
{
	soundProfile = "OssiStaffImpactSound";
	particleEmitter = "OssiStaffTrailEmitter";
	particleDensity = 25;
	particleRadius = 0.25;
	emitter[0] = "OssiStaffExplosionEmitter";
	emitter[1] = "OssiStaffTrailEmitter";
	lifetimeMS = 350;
	shakeCamera = false;
	lightStartRadius = 10;
	lightEndRadius = 0;
	lightStartColor = "1 1 0.5 1";
	lightEndColor = "0.5 0.5 1 1";
	damageRadius = 2;
	radiusDamage = 30;
};

datablock ProjectileData(OssiStaffProjectile)
{
	projectileShapeName = "base/data/shapes/empty.dts";
	particleEmitter = OssiStaffTrailEmitter;
	sound = EvilLoopSound;
	Explosion = OssiStaffExplosion;
	hasLight = true;
	lightRadius = 10;
	lightColor = "1 1 1";
	isBallistic = 1;
	velInheritFactor = 0;
	muzzleVelocity = 80;
	lifetime = 5000;
	armingDelay = 0;
	fadeDelay = 125;
	bounceAngle = 170;
	minStickVelocity = 10;
	ballRadius = 0.7;
	uiName = "OssiMS staff";
	bounceElasticity = 0.5;
	bounceFriction = 0;
	gravityMod = 0;
};

datablock ItemData(OssiStaffItem)
{
	category = "Weapon";
	className = "Weapon";

	shapeFile = "Add-Ons/GameMode_ReverieFortWars/models/Ossimetallisynthesis_Staff.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	uiName = "OssiMS staff";
	iconName = "Add-Ons/GameMode_ReverieFortWars/models/OssiStaff";
	doColorShift = false;
	colorShiftColor = "1 1 1 1";

	image = OssiStaffImage;
	canDrop = true;
};

datablock ShapeBaseImageData(OssiStaffImage)
{
	shapeFile = OssiStaffItem.shapeFile;
	emap = true;

	mountPoint = 0;
	offset = "0 0 0";

	correctMuzzleVector = false;

	eyeOffset = "0 0 0";

	className = "WeaponImage";

	item = OssiStaffItem;
	ammo = " ";
	projectile = OssiStaffProjectile;
	projectileType = projectile;

	melee = true;
	doRetraction = false;
	armReady = true;

	doColorShift = OssiStaffItem.doColorShift;
	colorShiftColor = OssiStaffItem.colorShiftColor;

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.5;
	stateTransitionOnTimeout[0] = "Ready";
	stateSound[0] = weaponSwitchSound;

	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "Fire";
	stateAllowImageChange[1] = true;

	stateName[2] = "Fire";
	stateFire[2] = true;
	stateScript[2] = "onFire";
	stateAllowImageChange[2] = false;
	stateTimeoutValue[2] = 1;
	stateWaitForTimeout[2] = true;
	stateTransitionOnTimeout[2] = "Ready";
};

// #4.6
datablock AudioProfile(BloodSplatSound)
{
	filename = "Add-Ons/Gamemode_ReverieFortWars/sounds/blood_splat.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock ParticleData(SanguiWandTrailParticle)
{
	dragCoefficient = 0;
	windCoefficient = 0;
	gravityCoefficient = 0.3;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 800;
	lifetimeVarianceMS = 100;
	spinSpeed = 0;
	spinRandomMin = 900;
	spinRandomMax = 900;
	useInvAlpha = true;
	textureName = "base/data/particles/dot";
	colors[0] = "0.8 0 0 1";
	colors[1] = "0.75 0 0 1";
	colors[2] = "0.7 0 0 1";
	colors[3] = "0.6 0 0 0";
	sizes[0] = 0.25;
	sizes[1] = 0.2;
	sizes[2] = 0.15;
	sizes[3] = 0.05;
	times[0] = 0;
	times[1] = 0.3;
	times[2] = 0.7;
	times[3] = 1;
};

datablock ParticleEmitterData(SanguiWandTrailEmitter)
{
	ejectionPeriodMS = 10;
	periodVarianceMS = 0;
	ejectionVelocity = 0.5;
	velocityVariance = 0.2;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = false;
	particles = "SanguiWandTrailParticle";
	uiName = "SanguiMS Wand Trail";
};

datablock ParticleData(SanguiWandExplosionParticle)
{
	dragCoefficient = 6;
	windCoefficient = 0;
	gravityCoefficient = 1.5;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 500;
	lifetimeVarianceMS = 100;
	spinSpeed = 0;
	spinRandomMin = -900;
	spinRandomMax = 900;
	useInvAlpha = true;
	textureName = "base/data/particles/dot";
	colors[0] = "0.8 0 0 1";
	colors[1] = "0.75 0 0 1";
	colors[2] = "0.7 0 0 1";
	colors[3] = "0.6 0 0 0";
	sizes[0] = 0.25;
	sizes[1] = 0.1;
	sizes[2] = 0.05;
	sizes[3] = 0.05;
	times[0] = 0;
	times[1] = 0.33;
	times[2] = 0.67;
	times[3] = 1;
};

datablock ParticleEmitterData(SanguiWandExplosionEmitter)
{
	ejectionPeriodMS = 7;
	periodVarianceMS = 0;
	ejectionVelocity = 10;
	velocityVariance = 5;
	ejectionOffset = 0.1;
	thetaMin = 0;
	thetaMax = 60;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = false;
	particles = "SanguiWandExplosionParticle";
	uiName = "SanguiMS Wand Explosion";
};

datablock ExplosionData(SanguiWandExplosion)
{
	soundProfile = BloodSplatSound;
	particleEmitter = SanguiWandExplosionEmitter;
	particleDensity = 25;
	particleRadius = 0.25;
	explosionScale = "1 1 1";
	playSpeed = 1;
	emitter[0] = SanguiWandExplosionEmitter;
	emitter[1] = SanguiWandTrailEmitter;
	debrisNum = 0;
	debrisNumVariance = 0;
	lifetimeMS = 250;
	lifetimeVariance = 50;
	shakeCamera = false;
	damageRadius = 0;
	radiusDamage = 0;
};

datablock ProjectileData(SanguiWandProjectile)
{
	projectileShapeName = "base/data/shapes/empty.dts";
	particleEmitter = SanguiWandTrailEmitter;
	sound = EvilLoopSound;
	Explosion = SanguiWandExplosion;
	hasLight = false;
	isBallistic = 1;
	velInheritFactor = 0;
	muzzleVelocity = 80;
	lifetime = 5000;
	armingDelay = 0;
	fadeDelay = 125;
	bounceAngle = 170;
	minStickVelocity = 10;
	uiName = "SanguiMS Wand";
	bounceElasticity = 0.5;
	bounceFriction = 0;
	gravityMod = 0.2;
};

function SanguiWandProjectile::onCollision(%this, %obj, %col, %fade, %pos, %normal)
{
	if(%col.getType() & $Typemasks::PlayerObjectType && minigameCanDamage(%obj.sourceObject, %col))
	{
		%col.damage(%obj.sourceObject, %pos, 40, $DamageType::SanguiWand);
		%col.sanguification+= 0.3;
		for(%i = 1; %i <= 5; %i++)
		{
			%col.schedule(%i * 1000, damage, %obj.sourceObject, %pos, 5, $DamageType::Bleed);
		}
	}
}

datablock ItemData(SanguiWandItem)
{
	category = "Weapon";
	className = "Weapon";

	shapeFile = "Add-Ons/GameMode_ReverieFortWars/models/Sanguimetallisynthesis_Wand.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	uiName = "SanguiMS wand";
	iconName = "Add-Ons/GameMode_ReverieFortWars/models/SanguiWand";
	doColorShift = false;
	colorShiftColor = "1 1 1 1";

	image = SanguiWandImage;
	canDrop = true;
};

datablock ShapeBaseImageData(SanguiWandImage)
{
	shapeFile = SanguiWandItem.shapeFile;
	emap = true;

	mountPoint = 0;
	offset = "0 0 0";

	correctMuzzleVector = false;

	eyeOffset = "0 0 0";

	className = "WeaponImage";

	item = SanguiWandItem;
	ammo = " ";
	projectile = SanguiWandProjectile;
	projectileType = projectile;

	melee = true;
	doRetraction = false;
	armReady = true;

	doColorShift = SanguiWandItem.doColorShift;
	colorShiftColor = SanguiWandItem.colorShiftColor;

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.5;
	stateTransitionOnTimeout[0] = "Ready";
	stateSound[0] = weaponSwitchSound;

	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "Prefire";
	stateAllowImageChange[1] = true;

	stateName[2] = "Prefire";
	stateTimeoutValue[2] = 0.13;
	stateTransitionOnTimeout[2] = "Fire";
	stateScript[2] = "onPrefire";

	stateName[3] = "Fire";
	stateFire[3] = true;
	stateScript[3] = "onFire";
	stateAllowImageChange[3] = false;
	stateTimeoutValue[3] = 0.6;
	stateTransitionOnTimeout[3] = "Ready";
};

function SanguiWandImage::onPrefire(%this, %obj, %slot)
{
	%obj.playThread(2, shiftAway);
}

function SanguiWandImage::onFire(%this, %obj, %slot)
{
	%obj.playThread(2, shiftTo);
	if(%obj.subtractMana(1))
	{
		%data = %this.projectile;
		%z = getWord(%obj.getScale(), 2);
		%proj = new Projectile()
		{
			datablock = %data;
			initialPosition = %obj.getMuzzlePoint(%slot);
			initialVelocity = vectorScale(%obj.getMuzzleVector(%slot), %data.muzzleVelocity);
			sourceObject = %obj;
			scale = %z SPC %z SPC %z;
		};
	}
}

// #5.

// #5.1

datablock AudioProfile(FaerieAmbientSound)
{
	filename = "Add-Ons/Gamemode_ReverieFortWars/sounds/faerie_loop.wav";
	description = AudioDefaultLooping3d;
	preload = true;
};

datablock AudioProfile(FaerieJumpSound)
{
	filename = "Add-Ons/Gamemode_ReverieFortWars/sounds/faerie_jump.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(FaerieHello)
{
	filename = "Add-Ons/Gamemode_ReverieFortWars/sounds/faerie_hello.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(FaerieWatchout)
{
	filename = "Add-Ons/Gamemode_ReverieFortWars/sounds/faerie_watchout.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock PlayerData(PlayerFaerie : ReveriePlayer)
{
	uiName = "Faerie";
	mass = 30;
	jumpForce = 500;
	minImpactSpeed = 500;
	jumpSound = FaerieJumpSound;
};

datablock ParticleData(FaerieParticle)
{
	dragCoefficient = 2;
	windCoefficient = 0.25;
	gravityCoefficient = 0;
	inheritedVelFactor = 0.4;
	constantAcceleration = 0;
	lifetimeMS = 500;
	lifetimeVarianceMS = 300;
	spinSpeed = 0;
	spinRandomMin = -360;
	spinRandomMax = 360;
	useInvAlpha = false;
	textureName = "base/data/particles/cloud";
	colors[0] = "0.1 0.6 0.3 0.4";
	colors[1] = "0.1 1   0.3 0.2";
	colors[2] = "0   0.6 0.3 0";
	sizes[0] = 1;
	sizes[1] = 1.2;
	sizes[2] = 0.5;
	times[0] = 0;
	times[1] = 0.2;
	times[2] = 1;
};

datablock ParticleEmitterData(FaerieEmitter)
{
	ejectionPeriodMS = 8;
	periodVarianceMS = 0;
	ejectionVelocity = 0.7;
	velocityVariance = 0.3;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	orientParticles = 0;
	orientOnVelocity = 1;
	particles = "FaerieParticle";
	useEmitterSizes = false;
	useEmitterColors = false;
	uiName = "Faerie";
};

datablock ShapeBaseImageData(FaerieImage)
{
	emap = false;
	shapeFile = "base/data/shapes/empty.dts";
	cloakable = true;
	mountPoint = $HeadSlot;
	firstPersonParticles = false;
	offset = "0 0 0";
	rotation = "1 0 0 0";
	eyeOffset = "0 0 0";
	eyeRotation = "1 0 0 0";
	firstPerson = true;
	usesEnergy = false;
	accuFire = false;
	lightType = "ConstantLight";
	lightColor = "0 0.6 0.3";
	lightTime = 1000;
	lightRadius = 20;

	stateName[0] = "Ready";
	stateTransitionOnTimeout[0] = "Fire";
	stateTimeoutValue[0] = 0.01;
	stateWaitForTimeout[0] = true;

	stateName[1] = "Fire";
	stateTransitionOnTimeout[1] = "Fire";
	stateTimeoutValue[1] = 10000;
	stateWaitForTimeout[1] = true;
	stateEmitter[1] = "FaerieEmitter";
	stateEmitterTime[1] = 10000;
	stateSound[1] = FaerieAmbientSound;
};

function FaerieImage::onMount(%this, %obj, %slot)
{
	%obj.hideNode("ALL");
}

// #5.2
datablock AudioProfile(MonsterGrowl)
{
	filename = "Add-Ons/Gamemode_ReverieFortWars/sounds/monster_growl.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock ShapeBaseImageData(RibcageImage)
{
	shapeFile = "Add-Ons/Gamemode_ReverieFortWars/models/ribcage.dts";
	emap = true;
	mountPoint = 2;
	offset = "0 0 -0.6";
	eyeOffset = "0 0 0.4";
	rotation = eulerToMatrix("0 0 0");
	scale = "1 1 1";
	doColorShift = false;
	colorShiftColor = "0 0.5 0.25 1";
};

// #5.3
datablock PlayerData(ReverieHorse : AdvancedHorseArmor)
{
	maxForwardSpeed = ReveriePlayer.maxForwardSpeed * 2; //vroom vroom
	maxSideSpeed = ReveriePlayer.maxSideSpeed * 0.5;
};

datablock AudioProfile(HorseNeigh)
{
	filename = "Add-Ons/Gamemode_ReverieFortWars/sounds/horse_neigh.wav";
	description = AudioDefault3d;
	preload = true;
};