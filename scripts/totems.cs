//ReverieFortWars/totems.cs

//TABLE OF CONTENTS

$RFW::TotemRadius = 100;

datablock fxDTSBrickData(totemBrick)
{
	brickFile = "./totem.blb";
	category = "Special";
	subCategory = "Reverie Fort Wars";
	uiName = "Totem";
	iconName = "base/client/ui/brickIcons/2x2x3";
	volume = 512;
};

function totemBrick::onPlant(%this, %obj)
{
	parent::onPlant(%this, %obj);
	if(!RegisterTotem(%obj))
	{
		return;
	}
	%obj.setEmitter(playerTeleportEmitterB);
	%obj.setLight(OrangeDimmerAmbientLight);
}

function totemBrick::onLoadPlant(%this, %obj)
{
	parent::onLoadPlant(%this, %obj);
	RegisterTotem(%obj);
}

function RegisterTotem(%obj)
{
	switch$($RFW::ResourceByColor[%obj.colorID])
	{
		case "Wood":
			%obj.damageReflection = 0.01;
			%obj.tierBonus = 0;
			%obj.HPmult = 2;
			%obj.HPregen = 0;
		case "Stone":
			%obj.damageReflection = 0.01;
			%obj.tierBonus = 1;
			%obj.HPmult = 2;
			%obj.HPregen = 0;
		case "Flesh":
			%obj.damageReflection = 0.02;
			%obj.tierBonus = 0;
			%obj.HPmult = 2;
			%obj.HPregen = 2;
		case "Surtra":
			%obj.damageReflection = 0.015;
			%obj.tierBonus = 0;
			%obj.HPmult = 4;
			%obj.HPregen = 0;
		case "Obsidian":
			%obj.damageReflection = 0.04;
			%obj.tierBonus = 0;
			%obj.HPmult = 2;
			%obj.HPregen = 0;
		case "Calcite":
			%obj.damageReflection = 0.03;
			%obj.tierBonus = 0;
			%obj.HPmult = 4;
			%obj.HPregen = 0;
		case "Nithlite":
			%obj.damageReflection = 0.03;
			%obj.tierBonus = 1;
			%obj.HPmult = 2;
			%obj.HPregen = 2;
		case "Sanguite":
			%obj.damageReflection = 0.035;
			%obj.tierBonus = 1;
			%obj.HPmult = 2;
			%obj.HPregen = 4;
		case "Sinnite":
			%obj.damageReflection = 0.03;
			%obj.tierBonus = 1;
			%obj.HPmult = 4;
			%obj.HPregen = 0;
		case "Crystal":
			%obj.damageReflection = 0.045;
			%obj.tierBonus = 1;
			%obj.HPmult = 4.5;
			%obj.HPregen = 2;
		default: 
			if(isObject(%client = %obj.getGroup().client))
			{
				%obj.damage(%client, "INF");
			}
			else
			{
				%obj.delete();
			}
			return false;
	}
	%brickGroup = %obj.getGroup();
	if(!strLen(%brickGroup.tribe) && isObject(%client = %brickGroup.client))
	{
		%brickGroup.tribe = %client.account.read("Tribe");
	}
	if(isObject(%oldTotem = %brickGroup.totem) && %oldTotem.isPlanted)
	{
		if(isObject(%client = %brickGroup.client))
			%oldTotem.damage(%client, "INF");
		else
			%oldTotem.delete();

		%brickGroup.totem = %obj;
	}
	return true;
}

// wood: 0.5% damage reflection
// stone: increases tier of tools needed to mine, 0.5% damage reflection
// flesh: brick HP regenerates over time, 1% damage reflection
// surtra: doubles brick HP, 0.8% damage reflection
// obsidian: 2% damage reflection
// calcite: doubles brick HP, 1.5% damage reflection
// sanguite: increases tier of tools needed to mine, brick HP regenerates over time, 2.5% damage reflection

function findClosestTotem(%pos)
{
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
	return %closestTotem;
}


function findClosestTotemDist(%pos)
{
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
			}
		}
	}
	return %closestTotemDist != $RFW::TotemRadius ? %closestTotemDist : -1;
}