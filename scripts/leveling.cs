$RFW::MaxLevel = 70;

function RFW_GenerateLevels()
{
	deleteVariables("$RFW::LevelStat*");
	$RFW::LevelStatHealth = 90;
	$RFW::LevelStatExp = 180;
	$RFW::LevelStatMana = 100;
	for(%i = 1; %i <= $RFW::MaxLevel; %i++)
	{
		$RFW::LevelStat[%i, "Health"] = $RFW::LevelStatHealth + (%i-1)*2.8;
		$RFW::LevelStat[%i, "Exp"] = $RFW::LevelStatExp + (%i-1)*13.3;
		$RFW::LevelStat[%i, "Mana"] = $RFW::LevelStatMana + (%i-1)*1.6;
	}
}
RFW_GenerateLevels();

package RFW_Leveling
{
	function GameConnection::createPlayer(%this, %transform)
	{
		cancel(%this.levelingSch);
		%this.levelingSch = %this.schedule(50, "RFW_Init");
		return Parent::createPlayer(%this, %transform);
	}
};
activatePackage("RFW_Leveling");

function GameConnection::RFW_Init(%this)
{
	if(!isObject(%player = %this.player))
		return;

	if(!isObject(%acc = %this.account))
		return;

	if(%acc.vHealth $= "")
		%acc.vHealth = getLevelStat(1, "hp");

	if(%acc.vMaxExp $= "")
		%acc.vMaxExp = getLevelStat(1, "exp");

	if(%acc.vExp $= "")
		%acc.vExp = 0;

	if(%acc.vLevel $= "")
		%acc.vLevel = 1;

	%player.setMaxHealth(%acc.vHealth);
}

function getLevelStat(%lv, %type)
{
	if(%lv > $RFW::MaxLevel)
		%lv = $RFW::MaxLevel;

	%stat = 0;

	%hpStart = 100;
	%hp = 2;

	%expStart = 50;
	%exp = 5;

	%lvLoop = %lv-1;
	if(%type $= "health" || %type $= "hp")
	{
		if(%lvLoop > 1)
		{
			for(%i = 0; %i < %lvLoop; %i++)
				%hpStart += %hp;
		}

		%stat = %hpStart;
	}
	else if(%type $= "exp" || %type $= "xp")
		return $RFW::LevelStat[%lv, ""];

	return 0;
}

function GameConnection::checkLevel(%this, %debug)
{
	if(!isObject(%acc = %this.account))
		return;

	if(%acc.vLevel <= 0)
		%acc.vLevel = 1;

	if(%acc.vExp < 0)
	{
		while(%acc.vExp < 0)
		{
			if(%acc.vLevel <= 1)
			{
				%acc.vExp = 0;
				break;
			}

			%acc.vLevel--;
			%exp = %acc.vExp += $RFW::LevelStat[%acc.vLevel, "exp"];
			%levels++;
		}

		%hp = %acc.vHealth = $RFW::LevelStat[%acc.vLevel, "health"];
		%maxExp = %acc.vMaxExp = $RFW::LevelStat[%acc.vLevel, "exp"];
		%mana = %acc.vMana = $RFW::LevelStat[%acc.vLevel, "mana"];

		if(%debug)
			%debugStr = "\c6(Debug -> Health " @ %hp @ " | Mana " @ %mana @ " | EXP " @ %exp @ "/" @ %maxExp @ "\c6)";

		messageAll('', '\c3%1 \c6has leveled down to \c3%2\c6. %3', %this.getPlayerName(), %acc.vLevel, %debugStr);
	}
	else if(%acc.vExp > %acc.vMaxExp)
	{
		while(%acc.vExp > %acc.vMaxExp)
		{
			if(%acc.vLevel >= $RFW::MaxLevel)
				break;

			%exp = %acc.vExp -= $RFW::LevelStat[%acc.vLevel, "exp"];
			%acc.vLevel++;

			%levels++;
		}

		%hp = %acc.vHealth = $RFW::LevelStat[%acc.vLevel, "health"];
		%mana = %acc.vMana = $RFW::LevelStat[%acc.vLevel, "mana"];
		%maxExp = %acc.vMaxExp = $RFW::LevelStat[%acc.vLevel, "exp"];

		if(%debug)
			%debugStr = "\c6(Debug -> Health " @ %hp @ " | Mana " @ %mana @ " | EXP " @ %exp @ "/" @ %maxExp @ "\c6)";

		messageAll('', '\c3%1 \c6has leveled up to \c3%2\c6. %3', %this.getPlayerName(), %acc.vLevel, %debugStr);

		if(isObject(%player = %this.player))
			%player.setMaxHealth(%hp);
	}
}

function GameConnection::addEXP(%this, %exp, %debug)
{
	if(!isObject(%acc = %this.account))
		return;

	%acc.vExp += %exp;
	%this.checkLevel(%debug);
}

function GameConnection::setLevel(%this, %level, %debug)
{
	if(!isObject(%acc = %this.account))
		return;

	if(%level > $RFW::MaxLevel)
		%level = $RFW::MaxLevel;

	%acc.vExp = 0;
	%hp = %acc.vHealth = $RFW::LevelStat[%level, "health"];
	%maxExp = %acc.vMaxExp = $RFW::LevelStat[%level, "exp"];
	%mana = %acc.vMana = $RFW::LevelStat[%level, "mana"];
	%acc.vLevel = %level;

	if(%debug)
		%debugStr = "\c6(Debug -> Health " @ %hp @ " | Mana " @ %mana @ " | EXP 0/" @ %maxExp @ "\c6)";

	messageAll('', '\c3%1 \c6has been set to level \c3%2\c6. %3', %this.getPlayerName(), %level, %debugStr);
	if(isObject(%player = %this.player))
		%player.setMaxHealth(%hp);
}