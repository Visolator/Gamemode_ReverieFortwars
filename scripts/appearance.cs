//ReverieFortWars/appearance.cs

//TABLE OF CONTENTS

$RFW::Skin[0] = "1 0.9 0.6 1"; //peach
$RFW::Skin[1] = "0.4 0.2 0 1"; //brown
$RFW::Skin[2] = "0.9 0.9 0 1"; //yellow
$RFW::Skin[3] = "1 0.9 0.8 1"; //light peach
$RFW::SkinColors = 4;
$RFW::SkinMaxDist = 0.15;

$RFW::Pants[0] = "0.4 0.2 0 1"; //brown
$RFW::Pants[1] = "0 0.5 0.25 1"; //green
$RFW::PantsColors = 2;
$RFW::PantsMaxDist = 0.15;

package RFW_Appearance
{
	function GameConnection::ApplyBodyColors(%client)
	{
		parent::ApplyBodyColors(%client);
		if(!isObject(%obj = %client.player))
		{
			return;
		}
		%acc = %client.account;
		%skin = %client.headColor;
		for(%i = 0; %i < $RFW::SkinColors; %i++)
		{
			if(vectorDist(%skin, $RFW::Skin[%i]) <= $RFW::SkinMaxDist)
			{
				%v = true;
				break;
			}
		}
		if(!%v)
		{
			%skin = $RFW::Skin[0];
		}
		%obj.setNodeColor("headSkin", %skin);
		if(%acc.read("Shirt") $= "")
		{
			%obj.setNodeColor($Chest[%client.chest], %skin);
			%obj.setNodeColor($LArm[%client.lArm], %skin);
			%obj.setNodeColor($RArm[%client.rArm], %skin);
			%obj.setDecalName("AAA-None");
		}
		else
		{
			%shirt = %client.chestColor !$= %skin ? %client.chestColor : "0 0.5 0.25 1";
			%obj.setNodeColor($Chest[%client.chest], %shirt);
			if(%client.lArmColor $= %client.chestColor)
			{
				%obj.setNodeColor($LArm[%client.lArm], %shirt);
			}
			else
			{
				%obj.setNodeColor($LArm[%client.lArm], %skin);
			}
			if(%client.rArmColor $= %client.chestColor)
			{
				%obj.setNodeColor($RArm[%client.rArm], %shirt);
			}
			else
			{
				%obj.setNodeColor($RArm[%client.rArm], %skin);
			}
		}
		%back = %acc.read("Back");
		if(getWord(getItemType(%back), 1) $= "quiver")
		{
			%obj.setNodeColor("Quiver", getColorIdTable($RFW::ResourceColor[firstWord(%back)]));
		}
		else if(getWord(getItemType(%back), 1) $= "cape" && %client.pack != 3)
		{
			%obj.setNodeColor("Cape", $RFW::TribeColor[%acc.read("Tribe")] SPC 1);
		}
		if(%acc.read("Hands") $= "")
		{
			if(%client.rHand != 1)
			{
				%obj.setNodeColor($RHand[%client.rHand], %skin);
			}
			else
			{
				%obj.setNodeColor($RHand[%client.rHand], getColorIdTable($RFW::ResourceColor["Surtra"]));
			}
			if(%client.lHand != 1)
			{
				%obj.setNodeColor($LHand[%client.lHand], %skin);
			}
			else
			{
				%obj.setNodeColor($LHand[%client.lHand], getColorIdTable($RFW::ResourceColor["Surtra"]));
			}
		}
		if(%acc.read("Feet") $= "")
		{
			if(%client.rLeg != 1)
			{
				%obj.setNodeColor($RLeg[%client.rLeg], %skin);
			}
			else
			{
				%obj.setNodeColor($RLeg[%client.rLeg], getColorIdTable($RFW::ResourceColor["Wood"]));
			}
			if(%client.lLeg != 1)
			{
				%obj.setNodeColor($LLeg[%client.lLeg], %skin);
			}
			else
			{
				%obj.setNodeColor($LLeg[%client.lLeg], getColorIdTable($RFW::ResourceColor["Wood"]));
			}
		}
		%obj.applyPants(%skin);
		%obj.schedule(0, applyArmor, %client.account.read("Armor"));
	}

	function GameConnection::ApplyBodyParts(%client)
	{
		parent::ApplyBodyParts(%client);
		if(!isObject(%obj = %client.player))
		{
			return;
		}
		%acc = %client.account;
		if(%acc.read("Hat") $= "" || %client.hat < 4)
		{
			%obj.hideNode($Hat[%client.hat]);
			%obj.hideNode(getWord($AccentsAllowed[$Hat[%client.hat]], %client.accent));
		}
		%obj.hideNode($Pack[%client.pack]);
		%obj.hideNode($SecondPack[%client.secondPack]);
		%back = %acc.read("Back");
		if(getWord(getItemType(%back), 1) $= "quiver")
		{
			%obj.unhideNode("Quiver");
		}
		else if(getWord(getItemType(%back), 1) $= "cape")
		{
			%obj.unhideNode("Cape");
		}
		if(getWord(getItemType(%acc.read("Shirt")), 1) $= "Robe" && %client.hip == 0)
		{
			%obj.hideNode("pants");
			%obj.hideNode("lShoe");
			%obj.hideNode("rShoe");
			%obj.unhideNode("skirtHip");
			%obj.unhideNode("skirtTrimRight");
			%obj.unhideNode("skirtTrimLeft");
		}
		else if(%client.hip == 1)
		{
			%obj.hideNode("skirtHip");
			%obj.hideNode("skirtTrimRight");
			%obj.hideNode("skirtTrimLeft");
			%obj.unhideNode("pants");
			%obj.unhideNode("lShoe");
			%obj.unhideNode("rShoe");
		}
	}
};
activatePackage(RFW_Appearance);

function Player::ApplyArmor(%obj, %armor)
{
	%obj.damageReduction = 1;
	%obj.speedFactors = "";
	if(%armor $= "")
	{
		%obj.AddSpeedFactor(1, 0);
		return;
	}
	switch$(%armor)
	{
		case "Wooden armor":
			%color = getColorIDtable($RFW::ResourceColor["Wood"]);
			%speed = 0.9;
			%dmg = 100 / 120;
			%mana = 100 / 100;
		case "Faerie armor":
			%color = "0 0.7 0.3 1";
			%speed = 1.1;
			%dmg = 100 / 170;
			%mana = 100 / 125;
		case "Bone armor":
			%color = getColorIDtable($RFW::ResourceColor["Bone"]);
			%speed = 0.9;
			%dmg = 100 / 140;
			%mana = 100 / 110;
		case "Stone armor":
			%color = getColorIDtable($RFW::ResourceColor["Stone"]);
			%speed = 0.8;
			%dmg = 100 / 170;
			%mana = 100 / 100;
		case "Surtra armor":
			%color = getColorIDtable($RFW::ResourceColor["Surtra"]);
			%speed = 0.9;
			%dmg = 100 / 160;
			%mana = 100 / 100;
		case "Calcite armor":
			%color = getColorIDtable($RFW::ResourceColor["Calcite"]);
			%speed = 1;
			%dmg = 100 / 180;
			%mana = 100 / 100;
		case "Nithlite armor":
			%color = getColorIDtable($RFW::ResourceColor["Nithlite"]);
			%speed = 1;
			%dmg = 100 / 180;
			%mana = 100 / 110;
		case "Sanguite armor":
			%color = getColorIDtable($RFW::ResourceColor["Sanguite"]);
			%speed = 1.1;
			%dmg = 100 / 200;
			%mana = 100 / 100;
		case "Sinnite armor":
			%color = getColorIDtable($RFW::ResourceColor["Sinnite"]);
			%speed = 1.1;
			%dmg = 100 / 200;
			%mana = 100 / 125;
		case "Crystal armor":
			%color = getColorIDtable($RFW::ResourceColor["Crystal"]);
			%speed = 1.2;
			%dmg = 100 / 250;
			%mana = 100 / 150;
	}
	%obj.AddSpeedFactor(%speed, 0);
	%obj.damageReduction = %dmg;
	%obj.manaReduction = %mana;
	%obj.setHeadUp(true);
	%obj.unhideNode("armor");
	%obj.setNodeColor("armor", %color);
	%obj.unhideNode("shoulderPads");
	%obj.setNodeColor("shoulderPads", %color);
	%obj.setNodeColor("pants", setWord(%color, 3, 1));
	%obj.setNodeColor("LShoe", setWord(%color, 3, 1));
	%obj.setNodeColor("RShoe", setWord(%color, 3, 1));
	if(isObject(%client = %obj.client))
	{
		if(%client.hat > 0 && %client.hat < 4)
		{
			%obj.unhideNode($Hat[%client.hat]);
			%obj.setNodeColor($Hat[%client.hat], %color);
		}
		if(%client.LHandColor !$= %client.headColor)
		{
			%obj.setNodeColor("LHand", setWord(%color, 3, 1));
		}
		if(%client.RHandColor !$= %client.headColor)
		{
			%obj.setNodeColor("RHand", setWord(%color, 3, 1));
		}
	}
}

function Player::ApplyShirt(%obj, %shirt)
{

}

function Player::ApplyPants(%obj, %skin)
{
	%client = %obj.client;
	%pants = %client.hipColor;
	%acc = %client.account;
	if(getWord(getItemType(%acc.read("Shirt")), 1) $= "Robe" && %client.hip == 0)
	{
		%obj.setNodeColor("skirtHip", %client.chestColor);
		%obj.setNodeColor("skirtTrimRight", %client.chestColor);
		%obj.setNodeColor("skirtTrimLeft", $RFW::TribeColor[%acc.read("Tribe")] SPC 1);
	}
	else if(%acc.read("Pants") $= "")
	{
		%v = false;
		for(%j = 0; %j < $RFW::PantsColors; %j++)
		{
			if(vectorDist(%pants, $RFW::Pants[%j]) <= $RFW::PantsMaxDist)
			{
				%v = true;
				break;
			}
		}
		if(!%v)
		{
			%pants = $RFW::Pants[0];
		}
		if(%pants $= %skin)
		{
			%pants = $RFW::Pants[1];
		}
		%obj.setNodeColor($Hip[0], %pants);
	}
	else
	{
		if(%pants $= %skin)
		{
			%pants = $RFW::Pants[2];
		}
		%obj.setNodeColor($Hip[0], %pants);
	}
}

function Player::ApplyHat(%obj, %hat)
{

}

function Player::ApplyBack(%obj, %back)
{

}