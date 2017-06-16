$RFW::WorldNames = "Astralis Oneirofortress Ciralen Luni Floren Tarsia Ivoni Ebori Shivan Seercrest Dreamridge Sagemont Vannil Choci Steerber Cherish Stardream Solhome";

datablock AudioProfile(RoosterSound)
{
	filename = "Add-Ons/GameMode_ReverieFortWars/sounds/OOT_6amRooster.wav";
	description = Audio2d;
	preload = true;
};

datablock AudioProfile(WolfSound)
{
	filename = "Add-Ons/GameMode_ReverieFortWars/sounds/OOT_6pmWolf.wav";
	description = Audio2d;
	preload = true;
};

function RFW_SetWorldName(%name)
{
	if(!strLen(%name))
	{
		$RFW::WorldName = getWord($RFW::WorldNames, getRandom(0, getWordcount($RFW::WorldNames) - 1));
	}
	else
	{
		$RFW::WorldName = %name;
	}
}

function RFW_StartDayTracking()
{
	cancel($RFW_Dawn);
	cancel($RFW_Dusk);
	if(!strLen($RFW::WorldName))
	{
		RFW_SetWorldName();
	}
	%time = $Sim::Time;
	%len = DayCycle.dayLength;
	%offset = %len * DayCycle.dayOffset;
	$RFW::DaysPassed = mFloor((%time - %offset) / %len);
	%rem = ($RFW::DaysPassed + 1) * %len + %offset - %time;
	$RFW_Dawn = schedule(%rem * 1000, 0, RFW_Dawn);
}

function RFW_Dawn()
{
	cancel($RFW_Dawn);
	cancel($RFW_Dusk);
	%clientCount = clientGroup.getCount();
	for(%i = 0; %i < %clientCount; %i++)
	{
		clientGroup.getObject(%i).play2d(RoosterSound);
	}
	$RFW::DaysPassed++;
	messageAll('', "\c6Day " @ $RFW::DaysPassed @ " dawns on " @ $RFW::WorldName @ ".");
	$RFW_Dawn = schedule(DayCycle.dayLength * 1000, 0, RFW_Dawn);
	$RFW_Dusk = schedule(DayCycle.dayLength * 500, 0, RFW_Dusk);
}

function RFW_Dusk()
{
	%clientCount = clientGroup.getCount();
	for(%i = 0; %i < %clientCount; %i++)
	{
		clientGroup.getObject(%i).play2d(WolfSound);
	}
}