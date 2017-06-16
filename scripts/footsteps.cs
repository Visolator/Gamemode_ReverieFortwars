for(%file = findFirstFile("Add-Ons/GameMode_ReverieFortWars/sounds/footsteps/*.wav"); isFile(%file); %file = findNextFile("Add-Ons/GameMode_ReverieFortWars/sounds/footsteps/*.wav"))
{
	%name = fileBase(%file);
	eval("datablock AudioProfile(" @ %name @ "){ filename = \"" @ %file @ "\"; description = AudioClose3d; preload = true; };");
}

$StepSounds["Carpet"] = 5;
$StepSounds["Dirt"] = 6;
$StepSounds["Grass"] = 7;
$StepSounds["JabuJabu"] = 7;
$StepSounds["Sand"] = 5;
$StepSounds["Stone"] = 6;
$StepSounds["Wood"] = 5;

function Player::GetFootstepType(%obj, %col)
{
	if(%col.getClassName() $= "fxDtsBrick")
	{
		%type = $RFW::ResourceByColor[%col.colorID];
		switch$(%type)
		{
			case "Plant matter": %type = "Grass";
			case "Snow": %type = "Sand";
			case "Flesh": %type = "JabuJabu";
			case "Surtra": %type = "Stone";
			case "Calcite": %type = "Stone";
			case "Nithlite": %type = "Stone";
			case "Sanguite": %type = "Stone";
			case "Sinnite": %type = "Stone";
			case "Crystal": %type = "Stone";
			case "Glass": %type = "Stone";
			case "Cloth": %type = "Carpet";
		}
		if(!$StepSounds[%type])
		{
			%type = "Dirt";
		}
	}
	else
	{
		%type = "Dirt";
	}
	return %type;
}

//! The package to handle when the player walks, and then play a noise.

package peggsteps
{
	//## When any creature spawns, start peggstepping.
	function Armor::onNewDatablock(%this, %obj)
	{
		if(%this.shapeFile !$= "base/data/shapes/player/m.dts" || %this.uiName $= "Faerie")
		{
			return parent::onNewDatablock(%this, %obj);
		}
		%obj.isSlow = 0;
		%obj.peggStep = schedule(50, 0, PeggFootsteps, %obj);
		%obj.peggJumpSounds = schedule(50, 0, PeggJumpSounds, %obj);
		return parent::onNewDatablock(%this, %obj);
	}

	function Armor::onTrigger(%data,%obj,%slot,%val)
	{
		if(%slot == 3)
		{
			%obj.isProning = %val;
		}
		return Parent::onTrigger(%data,%obj,%slot,%val);
	}

	function Armor::onEnterLiquid(%data, %obj)
	{
		%obj.isSwimming = true;
		return Parent::onEnterLiquid(%data, %obj);
	}
	function Armor::onLeaveLiquid(%data, %obj)
	{
		%obj.isSwimming = false;
		return Parent::onLeaveLiquid(%data,%obj);
	}
};
activatepackage(peggsteps);

//## Drop some rad peggstep noise in here!
function PeggFootsteps(%obj)
{
	if(isObject(%obj))
	{
		%schedlen = 320;
		%start = %obj.getPosition();
		%end = vectorAdd(%start, "0 0 -0.1");
		%typemasks = $Typemasks::FxBrickAlwaysObjectType | $Typemasks::TerrainObjectType | $Typemasks::PlayerObjectType;
		%ray = containerRaycast(%start, %end, %typemasks, %obj);
		%col = firstWord(%ray);
		if(isObject(%col))
		{	
			if(%col.getClassName() $= "fxDTSbrick")
			{
				%obj.touchcolor = %col.getColorId();
			}
			%vel = %obj.getVelocity();
			if(vectorLen(setWord(%vel, 2, 0)) > 0)
			{
				%type = %obj.getFootstepType(%col);
				if(!%obj.isProning)
				{
					%sound = "OOT_Steps_" @ %type @ getRandom(1, $StepSounds[%type]);
					serverplay3d(%sound, %obj.getHackPosition());
				}
				else
				{
					%time = getSimTime();
					if(%obj.crawltime + 850 < %time)
					{
						%sound = "OOT_Steps_PushCrawl_" @ %type;
						serverplay3d(%sound, %obj.getPosition());
						%obj.crawlTime = %time;
					}
				}
			}
		}
		%obj.peggstep = schedule(320, 0, PeggFootsteps, %obj);
	}
}

function PeggJumpSounds(%obj)
{
	if(!isObject(%obj))
	{
		return;
	}
	//%obj.tbp();
	cancel(%obj.peggJumpSounds);
	%pvel = %obj.prevVelocity;
	%cvel = %obj.getVelocity();
	%obj.prevVelocity = %cvel;
	%pz = getWord(%pvel, 2);
	%cz = getWord(%cvel, 2);
	if(%cz > 8 && %pz < 1)
	{
		//jumping
		%start = %obj.getPosition();
		%end = vectorAdd(%start, "0 0 -1.5");
		%typemasks = $Typemasks::FxBrickAlwaysObjectType | $Typemasks::TerrainObjectType | $Typemasks::PlayerObjectType;
		%ray = containerRaycast(%start, %end, %typemasks, %obj);
		%col = firstWord(%ray);
		if(isObject(%col))
		{
			%type = %obj.getFootstepType(%col);
			%sound = "OOT_Steps_" @ %type @ "_Jump" @ getRandom(1, 2);
			serverplay3d(%sound, %obj.getHackPosition());
		}
	}
	else if(%cz > -1 && %pz < -4)
	{
		//landing
		%start = %obj.getPosition();
		%end = vectorAdd(%start, "0 0 -0.1");
		%typemasks = $Typemasks::FxBrickAlwaysObjectType | $Typemasks::TerrainObjectType | $Typemasks::PlayerObjectType;
		%ray = containerRaycast(%start, %end, %typemasks, %obj);
		%col = firstWord(%ray);
		if(isObject(%col))
		{
			%type = %obj.getFootstepType(%col);
			%sound = "OOT_Steps_" @ %type @ "_Land" @ getRandom(1, 2);
			serverplay3d(%sound, %obj.getHackPosition());
		}
	}
	%obj.peggJumpSounds = schedule(50, 0, PeggJumpSounds, %obj);
}