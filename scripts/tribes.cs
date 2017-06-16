//ReverieFortWars/tribes.cs

//TABLE OF CONTENTS
// #0. party stuff
// #1. tribe stuff
// #2. package

// #0.

function RFW_AddTribe(%name, %color)
{
	$RFW::TribeColor[%name] = hex2dec(%color);
	$RFW::TribeColorHex[%name] = %color;
}

RFW_AddTribe("Malka", "BE0119");
RFW_AddTribe("Prelkan", "FE2C54");
RFW_AddTribe("Kazezi", "82CAFC");
RFW_AddTribe("Faeborn", "15B01A");
RFW_AddTribe("Sarti", "FFFF00");//D1B26F
RFW_AddTribe("Eristia", "9B7A01");
RFW_AddTribe("Mediir", "FFF39A");
RFW_AddTribe("Munaa", "F97306");
RFW_AddTribe("", "FFFFFF");
//RFW_AddParty("White", hex2dec("#ffffff"));
//RFW_AddParty("Red", hex2dec("#e50000"));
//RFW_AddParty("Blue", hex2dec("#0165fc"));
//RFW_AddParty("Green", hex2dec("#15b01a"));
//RFW_AddParty("Yellow", hex2dec("#ffff00"));
//RFW_AddParty("Purple", hex2dec("#9a0eea"));
//RFW_AddParty("Magenta", hex2dec("ff00ff"));
//RFW_AddParty("Cyan", hex2dec("#00ffff"));
//RFW_AddParty("Blood", hex2dec("#be0119"));
//RFW_AddParty("Bone", hex2dec("#fff39a"));
//RFW_AddParty("Bile", hex2dec("#9dff00"));
//RFW_AddParty("Brown", hex2dec("#9b7a01"));
//RFW_AddParty("Tan", hex2dec("#d1b26f"));
//RFW_AddParty("Lilac", hex2dec("#cea2fd"));
//RFW_AddParty("Turquoise", hex2dec("#06b1c4"));
//RFW_AddParty("Jade", hex2dec("#1fa774"));
//RFW_AddParty("Sky", hex2dec("#82cafc"));
//RFW_AddParty("Pink", hex2dec("#ffbacd"));
//RFW_AddParty("Gum", hex2dec("#ff6cb5"));
//RFW_AddParty("Mint", hex2dec("#8fff9f"));
//RFW_AddParty("Orange", hex2dec("#f97306"));
//RFW_AddParty("Silver", hex2dec("#c5c9c7"));
//RFW_AddParty("Steel", hex2dec("#607c8e"));
//RFW_AddParty("Stone", hex2dec("#ada587"));
//RFW_AddParty("Umber", hex2dec("#b26400"));
//RFW_AddParty("Flesh", hex2dec("#fe2c54"));
//RFW_AddParty("Rose", hex2dec("#cf6275"));
//RFW_AddParty("Ochre", hex2dec("#bf9005"));
//RFW_AddParty("Charcoal", hex2dec("#343837"));
//RFW_AddParty("Cobalt", hex2dec("#2c6fbb"));
//RFW_AddParty("Coral", hex2dec("#ff0789"));
//RFW_AddParty("Peach", hex2dec("#ffb07c"));
//RFW_AddParty("Pear", hex2dec("#cbf85f"));

$RFW::ActiveTribes = "Sarti Mediir";

// #1.

package RFW_Chat
{
	function chatMessageAll(%client, %msgString, %clanPrefix, %name, %clanSuffix, %msg)
	{
		%tribe = %client.account.read("Tribe");
		%color = "<color:" @ $RFW::TribeColorHex[%tribe] @ ">";
		%name = %color @ %name;
		%clanSuffix = "<spush><font:Symbol:24>\c7" @ %clanPrefix @ %clanSuffix @ "<spop>";
		%clanPrefix = %tribe;
		%msgString = '\c7%1%2%3\c6: %4';
		return parent::chatMessageAll(%client, %msgString, %clanPrefix, %name, %clanSuffix, %msg);
	}

	function chatMessageTeam(%client, %a, %msgString, %clanPrefix, %name, %clanSuffix, %msg)
	{
		%tribe = %client.account.read("Tribe");
		if(%tribe $= "")
			return parent::chatMessageTeam(%client, %a, %msgString, %clanPrefix, %name, %clanSuffix, %msg);

		%color = "<color:" @ $RFW::TribeColorHex[%tribe] @ ">";
		%name = %color @ %name;
		%clanSuffix = "<spush><font:Symbol:24>\c7" @ %clanPrefix @ %clanSuffix @ "<spop>";
		%clanPrefix = %color @ %tribe;
		%msgString = '\c7[%1\c7]%2%3\c4: %4';
		%clientCount = clientGroup.getCount();
		//echo("clanPrefix:" @ %clanPrefix);
		//echo("clanSuffix:" @ %clanSuffix);
		//echo("name:" @ %name);
		for(%i = 0; %i < %clientCount; %i++)
		{
			%cl = clientGroup.getObject(%i);
			if(%cl.account.read("Tribe") $= %tribe)
				commandToClient(%cl, 'chatMessage', %client, "", "", %msgString, %clanPrefix, %name, %clanSuffix, %msg);
		}
	}
};
activatePackage(RFW_Chat);

// #2.

package RFW_Tribes
{
	function GameConnection::SpawnPlayer(%client)
	{
		%p = parent::SpawnPlayer(%client);
		if(isObject(%obj = %client.player))
		{
			%obj.setDatablock(ReveriePlayer);
			%obj.setEnergyLevel(0);
			commandToClient(%client, 'showEnergyBar', true);
			%obj.setShapeNameDistance(32);
			%obj.setShapeNameColor($RFW::TribeColor[%client.account.read("Tribe")]);
		}
		return %p;
	}
};
activatePackage(RFW_Tribes);

function serverCmdSwitchTribe(%client, %newtribe)
{
	if(!strLen($RFW::TribeColor[%newtribe]))
	{
		messageClient(%client, 'MsgError');
		%client.centerPrintQueue("<color:FF0000>Could not find tribe '" @ %newtribe @ "'", 3000);
		return;
	}
	%oldtribe = %client.account.read("Tribe");
	%newtribe = strUpr(getSubStr(%newtribe, 0, 1)) @ strLwr(getSubStr(%newtribe, 1, strLen(%newtribe)));
	%client.account.delete();
	%acc = RFW_NewAccount();
	%client.account = %acc;
	%acc.client = %client;
	%acc.set("Tribe", %newtribe);
	serverCmdClearBricks(%client);
	if(isObject(%client.player))
	{
		%client.instantRespawn();
	}
	messageAll('', "\c3" @ %client.name @ "\c5 has defected from the <color:" @ $RFW::TribeColorHex[%oldtribe] @ ">" @ %oldtribe @ "\c5 to the <color:" @ $RFW::TribeColorHex[%newtribe] @ ">" @ %newtribe @ "\c5 tribe!");
}