//ReverieFortWars/religion.cs

//TABLE OF CONTENTS

//first up: mediir and sarti
//next up: kazezi and eristia

$RFW::KillsFavorBonus["Mediir", "Bone"] = 0.5;
$RFW::KillsFavorBonus["Mediir", "Calcite"] = 1;

datablock fxDTSBrickData(brickAltarData)
{
	brickFile = "./altar.blb";
	category = "Special";
	subCategory = "Reverie Fort Wars";
	uiName = "Altar";
	//iconName = "Add-Ons/GameMode_ReverieFortWars/models/brick16x16";
};

package RFW_Religion
{
	function gameConnection::onDeath(%this, %sObj, %sClient, %dmgType, %dmgArea)
	{
		%p = parent::onDeath(%this, %sObj, %sClient, %dmgType, %dmgArea);
		if(isObject(%sClient))
		{
			RFW_ParseKill(%this, %sClient);
		}
		return %p;
	}
};
activatePackage(RFW_Religion);

function RFW_ParseKill(%vClient, %kClient)
{
	%vAcc = %vClient.account;
	%kAcc = %kClient.account;
	//check if this was a dishonorable kill
	//%honorable = %kAcc.read("EquipmentTier") <= %vAcc.read("EquipmentTier");
	//for now..
	%honorable = true;
	%tribe = %kAcc.read("Tribe");
	%player = %kClient.player;
	%slot = %player.currTool;
	if(isObject(%player) && %slot != -1)
	{
		//record kill on weapon
		%wpn = incItemKills(%kAcc.read("Equip" @ %slot), 1);
		%kAcc.set("Equip" @ %slot, %wpn);
		//check for favor bonus for this weapon
		if(%honorable)
		{
			%mat = firstWord(getItemType(%wpn));
			%multipler = 1 + $RFW::KillsFavorBonus[%tribe, %mat];
			%favor = 10 * %multiplier;
			%kAcc.addFavor(%favor);
		}
	}
}