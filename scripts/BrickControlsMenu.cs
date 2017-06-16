//////////////////////
//rfw menu specifics//
//////////////////////
$BCM::DefaultMenu = "RFW";

//initial menu
$BCM::Menu["RFW", 1] = "static" SPC "Inventory\nEquipment\nCrafting\nOptions\nHelp";
$BCM::Transitions["RFW", 1] = "2 3 4 5 6";
$BCM::PrevState["RFW", 1] = "0";

//inventory
$BCM::Menu["RFW", 2] = "function" SPC "%client.account.getItemList();";
$BCM::EnterCallback["RFW", 2] = "serverCmdEquip(%client, %selection); %client.BCM_MenuChangeState(%state);";
$BCM::ScrollCallback["RFW", 2] = "Menu_SetQuantity(%client, %selection);";
$BCM::EnableQuantity["RFW", 2] = true;
$BCM::QuantityCallback["RFW", 2] = "Menu_VerifyQuantity(%client, %quantity);";
$BCM::PrevState["RFW", 2] = "1";

//equipment
$BCM::Menu["RFW", 3] = "function" SPC "%client.account.getEquipmentList();";
$BCM::EnterCallback["RFW", 3] = "serverCmdUnequip(%client, %selection); %client.BCM_MenuChangeState(%state);";
$BCM::PrevState["RFW", 3] = "1";

//crafting
$BCM::Menu["RFW", 4] = "static" SPC "Materials\nWeapons and tools\nArmor\nClothes\nMedicines and potions";
$BCM::Transitions["RFW", 4] = "7 8 18 17 19";
$BCM::PrevState["RFW", 4] = "1";

//crafting > materials
$BCM::Menu["RFW", 7] = "static" SPC "Cloth\nPaper\nLeather\nGlass\nStone\nCalcite\nSanguite";
$BCM::DisplayFunction["RFW", 7] = "Menu_DisplayCrafting(%client, %menu, %position);";
$BCM::EnableQuantity["RFW", 7] = true;
$BCM::DefaultQuantity["RFW", 7] = 1;
$BCM::EnterCallback["RFW", 7] = "serverCmdCraft(%client, %quantity, %selection); %client.BCM_MenuDisplay();";
$BCM::PrevState["RFW", 7] = "4";

//crafting > weapons and tools
$BCM::Menu["RFW", 8] = "static" SPC "Pickaxes\nCrafting tools\nSwords\nDaggers\nBows\nQuivers\nMagic tools\nOther";
$BCM::Transitions["RFW", 8] = "9 10 11 12 13 14 15 16";
$BCM::PrevState["RFW", 8] = "4";

//crafting > weapons and tools > pickaxes
$BCM::Menu["RFW", 9] = "static" SPC "Stone pickaxe\nSurtra pickaxe\nCalcite pickaxe\nNithlite pickaxe\nSanguite pickaxe\nSinnite pickaxe\nCrystal pickaxe";
$BCM::DisplayFunction["RFW", 9] = "Menu_DisplayCrafting(%client, %menu, %position);";
$BCM::EnableQuantity["RFW", 9] = true;
$BCM::DefaultQuantity["RFW", 9] = 1;
$BCM::EnterCallback["RFW", 9] = "serverCmdCraft(%client, %quantity, %selection); %client.BCM_MenuDisplay();";
$BCM::PrevState["RFW", 9] = "8";

//crafting > weapons and tools > crafting tools
$BCM::Menu["RFW", 10] = "static" SPC "Wooden crafting tools\nStone crafting tools\nObsidian crafting tools\nSurtra crafting tools\nCalcite crafting tools\nNithlite crafting tools\nSanguite crafting tools\nSinnite crafting tools";
$BCM::DisplayFunction["RFW", 10] = "Menu_DisplayCrafting(%client, %menu, %position);";
$BCM::EnableQuantity["RFW", 10] = true;
$BCM::DefaultQuantity["RFW", 10] = 1;
$BCM::EnterCallback["RFW", 10] = "serverCmdCraft(%client, %quantity, %selection); %client.BCM_MenuDisplay();";
$BCM::PrevState["RFW", 10] = "8";

//crafting > weapons and tools > swords
$BCM::Menu["RFW", 11] = "static" SPC "Wooden sword\nStone sword\nObsidian sword\nSurtra sword\nCalcite sword\nNithlite sword\nSanguite sword\nSinnite sword\nCrystal sword";
$BCM::DisplayFunction["RFW", 11] = "Menu_DisplayCrafting(%client, %menu, %position);";
$BCM::EnableQuantity["RFW", 11] = true;
$BCM::DefaultQuantity["RFW", 11] = 1;
$BCM::EnterCallback["RFW", 11] = "serverCmdCraft(%client, %quantity, %selection); %client.BCM_MenuDisplay();";
$BCM::PrevState["RFW", 11] = "8";

//crafting > weapons and tools > daggers
$BCM::Menu["RFW", 12] = "static" SPC "Obsidian dagger\nSurtra dagger\nCalcite dagger\nNithlite dagger\nSanguite dagger\nSinnite dagger\nCrystal dagger";
$BCM::DisplayFunction["RFW", 12] = "Menu_DisplayCrafting(%client, %menu, %position);";
$BCM::EnableQuantity["RFW", 12] = true;
$BCM::DefaultQuantity["RFW", 12] = 1;
$BCM::EnterCallback["RFW", 12] = "serverCmdCraft(%client, %quantity, %selection); %client.BCM_MenuDisplay();";
$BCM::PrevState["RFW", 12] = "8";

//crafting > weapons and tools > bows
$BCM::Menu["RFW", 13] = "static" SPC "Wooden bow\nBone bow\nFaerie bow\nCrystal bow";
$BCM::DisplayFunction["RFW", 13] = "Menu_DisplayCrafting(%client, %menu, %position);";
$BCM::EnableQuantity["RFW", 13] = true;
$BCM::DefaultQuantity["RFW", 13] = 1;
$BCM::EnterCallback["RFW", 13] = "serverCmdCraft(%client, %quantity, %selection); %client.BCM_MenuDisplay();";
$BCM::PrevState["RFW", 13] = "8";

//crafting > weapons and tools > quivers
$BCM::Menu["RFW", 14] = "static" SPC "Wooden quiver\nBone quiver\nStone quiver\nObsidian quiver\nSurtra quiver\nCalcite quiver\nNithlite quiver\nSanguite quiver\nSinnite quiver\nCrystal quiver";
$BCM::DisplayFunction["RFW", 14] = "Menu_DisplayCrafting(%client, %menu, %position);";
$BCM::EnableQuantity["RFW", 14] = true;
$BCM::DefaultQuantity["RFW", 14] = 1;
$BCM::EnterCallback["RFW", 14] = "serverCmdCraft(%client, %quantity, %selection); %client.BCM_MenuDisplay();";
$BCM::PrevState["RFW", 14] = "8";

//crafting > weapons and tools > magic tools
$BCM::Menu["RFW", 15] = "static" SPC "Faerie crafting tools\nFaerie wand\nPyromancy wand\nAlchemy vials\nPhotometallisynthesis lens\nOssimetallisynthesis staff\nSanguimetallisynthesis wand";
$BCM::DisplayFunction["RFW", 15] = "Menu_DisplayCrafting(%client, %menu, %position);";
$BCM::EnableQuantity["RFW", 15] = true;
$BCM::DefaultQuantity["RFW", 15] = 1;
$BCM::EnterCallback["RFW", 15] = "serverCmdCraft(%client, %quantity, %selection); %client.BCM_MenuDisplay();";
$BCM::PrevState["RFW", 15] = "8";

//crafting > weapons and tools > other
$BCM::Menu["RFW", 16] = "static" SPC "Snowball";
$BCM::DisplayFunction["RFW", 16] = "Menu_DisplayCrafting(%client, %menu, %position);";
$BCM::EnableQuantity["RFW", 16] = true;
$BCM::DefaultQuantity["RFW", 16] = 1;
$BCM::EnterCallback["RFW", 16] = "serverCmdCraft(%client, %quantity, %selection); %client.BCM_MenuDisplay();";
$BCM::PrevState["RFW", 16] = "8";

//crafting > clothes
$BCM::Menu["RFW", 17] = "static" SPC "Cloth hat\nCloth shirt\nCloth pants\nCloth cape\nCloth robe\nLeather boots\nLeather gloves";
$BCM::DisplayFunction["RFW", 17] = "Menu_DisplayCrafting(%client, %menu, %position);";
$BCM::EnableQuantity["RFW", 17] = true;
$BCM::DefaultQuantity["RFW", 17] = 1;
$BCM::EnterCallback["RFW", 17] = "serverCmdCraft(%client, %quantity, %selection); %client.BCM_MenuDisplay();";
$BCM::PrevState["RFW", 17] = "4";

//crafting > armor
$BCM::Menu["RFW", 18] = "static" SPC "Wooden armor\nFaerie armor\nBone armor\nStone armor\nSurtra armor\nCalcite armor\nNithlite armor\nSanguite armor\nSinnite armor\nCrystal armor";
$BCM::DisplayFunction["RFW", 18] = "Menu_DisplayCrafting(%client, %menu, %position);";
$BCM::EnableQuantity["RFW", 18] = true;
$BCM::DefaultQuantity["RFW", 18] = 1;
$BCM::EnterCallback["RFW", 18] = "serverCmdCraft(%client, %quantity, %selection); %client.BCM_MenuDisplay();";
$BCM::PrevState["RFW", 18] = "4";

//crafting > medicines and potions
$BCM::Menu["RFW", 19] = "static" SPC "Cloth bandage\nPishelva\nHoborchakin\nFleshweaver's potion\nVorpal potion\nCrimson potion";
$BCM::DisplayFunction["RFW", 19] = "Menu_DisplayCrafting(%client, %menu, %position);";
$BCM::EnableQuantity["RFW", 19] = true;
$BCM::DefaultQuantity["RFW", 19] = 1;
$BCM::EnterCallback["RFW", 19] = "serverCmdCraft(%client, %quantity, %selection); %client.BCM_MenuDisplay();";
$BCM::PrevState["RFW", 19] = "4";

//options
$BCM::Menu["RFW", 5] = "function" SPC "%client.account.getOptions();";
//$BCM::EnterCallback["RFW", 5] = "";
//$BCM::QuantityCallback["RFW", 5] = "";
//$BCM::EnableQuantity["RFW", 5] = true;
$BCM::PrevState["RFW", 5] = "1";

//help
$BCM::Menu["RFW", 6] = "static" SPC "Getting started\nItems\nCrafting\nResources\nBiomes\nBuilding\nTotems\nTribes\nForum thread"; //help
$BCM::EnterCallback["RFW", 6] = "serverCmdHelp(%client, %selection);";
$BCM::PrevState["RFW", 6] = "1";

//helper functions
function Menu_SetQuantity(%client, %selection)
{
	%client.BCM_MenuQuantity = %client.account.getItemQuantity(%selection);
}

function Menu_VerifyQuantity(%client, %quantity)
{
	%acc = %client.account;
	%selection = getRecord(%client.BCM_MenuCache, %client.BCM_MenuPosition);
	%amt = %acc.getItemQuantity(%selection);
	if(%amt < %quantity)
	{
		messageClient(%client, 'MsgError');
		%client.BCM_MenuQuantity = %amt;
	}
}

function Menu_DisplayCrafting(%client, %menu, %position)
{
	%acc = %client.account;
	%quantity = %client.BCM_MenuQuantity;
	%init = mClamp(%position - 2, 0, getRecordCount(%menu) - 5);
	%position-= %init;
	%rec = getRecord(%menu, %init);
	%color = %acc.hasRequiredItems($RFW::RecipeMats[%rec], $RFW::RecipeTools[%rec], %quantity) ? "\c2" : "\c0";
	%print = %color @ %rec;
	for(%i = %init + 1; %i < %init + 5; %i++)
	{
		%rec = getRecord(%menu, %i);
		%color = %acc.hasRequiredItems($RFW::RecipeMats[%rec], $RFW::RecipeTools[%rec], %quantity) ? "\c2" : "\c0";
		if(%client.canCraftAnything)
			%color = "\c2";
		
		%print = %print NL %color @ %rec;
	}
	%print = setRecord(%print, %position, "\c3>>" @ getRecord(%print, %position) @ "\c3x" @ %quantity);
	return %print;
}

////////////////////////
//BrickControlsMenu.cs//
////////////////////////
//by Amadé (ID 10716)

$BCM::Version = 1;

$BCM::Up1 = "0 0 1"; //shift up 1 plate
$BCM::Up3 = "0 0 3"; //shift up 1 block
$BCM::Down1 = "0 0 -1"; //shift down 1 plate
$BCM::Down3 = "0 0 -3"; //shift down 1 block
$BCM::Right = "1 0 0";
$BCM::Left = "-1 0 0";
$BCM::Forward = "0 1 0";
$BCM::Back = "0 -1 0";

//helper functions
function hasColor(%str)
{
	if(%str $= "")
	{
		return false;
	}
	%firstChar = getSubStr(%str, 0, 1);
	if(%firstChar $= "<")
	{
		return true;
	}
	return %firstChar $= "\c0" || %firstChar $= "\c1" || %firstChar $= "\c2" || %firstChar $= "\c3";
}

function BCM_GetMenuText(%str, %client)
{
	switch$(firstWord(%str))
	{
		case "static":
			return restWords(%str);
		//case "variable":
			//eval("%str = " @ restWords(%str));
			//return %str;
		case "function":
			eval("%str = " @ restWords(%str));
			return %str;
		default:
			return %str;
	}
}

//the main attraction
package BrickControlsMenu
{
	function serverCmdPlantBrick(%client)
	{
		if(!isObject(%client.player.tempBrick))
		{
			%client.BCM_MenuEnter();
		}
		else return parent::serverCmdPlantBrick(%client);
	}

	function serverCmdCancelBrick(%client)
	{
		if(!isObject(%client.player.tempBrick))
		{
			%client.BCM_MenuCancel();
		}
		else return parent::serverCmdCancelBrick(%client);
	}

	function serverCmdShiftBrick(%client, %y, %x, %z)
	{
		//y+: forward
		//x+: left
		//z+: up
		if(!isObject(%client.player.tempBrick))
		{
			%client.BCM_MenuShift(-1 * %x, %y, %z);
		}
		else return parent::serverCmdShiftBrick(%client, %y, %x, %z);
	}

	function serverCmdSuperShiftBrick(%client, %y, %x, %z)
	{
		if(!isObject(%client.player.tempBrick))
		{
			%client.BCM_MenuShift(-1 * %x, %y, %z);
		}
		else return parent::serverCmdSuperShiftBrick(%client, %y, %x, %z);
	}
};
activatePackage(BrickControlsMenu);

function GameConnection::BCM_MenuEnter(%client)
{
	if(!strLen(%profile = %client.BCM_MenuProfile))
	{
		%client.BCM_MenuProfile = %profile = $BCM::DefaultMenu;
	}
	%state = %client.BCM_MenuState;
	if(%state == 0)
	{
		return %client.BCM_MenuChangeState(1);
	}
	%position = %client.BCM_MenuPosition;
	if((%fn = $BCM::EnterCallback[%profile, %state]) !$= "")
	{
		if($BCM::EnableQuantity[%profile, %state])
		{
			%quantity = %client.BCM_MenuQuantity;
		}
		%menu = %client.BCM_MenuCache;
		%selection = stripMLControlChars(getRecord(%menu, %position));
		eval(%fn);
	}
	if((%transitions = $BCM::Transitions[%profile, %state]) !$= "")
	{
		%client.BCM_MenuChangeState(getWord(%transitions, %position));
	}
}

function GameConnection::BCM_MenuCancel(%client)
{
	if(isEventPending(%event = %client.BCM_MenuCancelSched))
	{
		cancel(%event);
		%client.centerPrint();
	}
	%client.BCM_MenuState = 0;
}

function GameConnection::BCM_MenuShift(%client, %x, %y, %z)
{
	if(!strLen(%profile = %client.BCM_MenuProfile))
	{
		%client.BCM_MenuProfile = %profile = $BCM::DefaultMenu;
	}
	%state = %client.BCM_MenuState;
	if(!%state)
	{
		return %client.BCM_MenuEnter;
	}
	switch$(%x SPC %y SPC %z)
	{
		case $BCM::Forward:
			return %client.BCM_MenuScroll(-1);
		case $BCM::Back:
			return %client.BCM_MenuScroll(1);
		case $BCM::Right:
			return %client.BCM_MenuEnter();
		case $BCM::Left:
			return %client.BCM_MenuBack();
		case $BCM::Up1:
			if($BCM::EnableQuantity[%profile, %state])
			{
				return %client.BCM_MenuAddQuantity(1);
			}
			else
			{
				return %client.BCM_MenuScroll(-1);
			}
		case $BCM::Up3:
			if($BCM::EnableQuantity[%profile, %state])
			{
				return %client.BCM_MenuAddQuantity(10);
			}
			else
			{
				return %client.BCM_MenuScroll(-1);
			}
		case $BCM::Down1:
			if($BCM::EnableQuantity[%profile, %state])
			{
				return %client.BCM_MenuAddQuantity(-1);
			}
			else
			{
				return %client.BCM_MenuScroll(1);
			}
		case $BCM::Down3:
			if($BCM::EnableQuantity[%profile, %state])
			{
				return %client.BCM_MenuAddQuantity(-10);
			}
			else
			{
				return %client.BCM_MenuScroll(1);
			}
	}
}

function GameConnection::BCM_MenuBack(%client)
{
	%profile = %client.BCM_MenuProfile;
	%state = %client.BCM_MenuState;
	%client.BCM_MenuChangeState($BCM::PrevState[%profile, %state]);
}

function GameConnection::BCM_MenuScroll(%client, %dir)
{
	%profile = %client.BCM_MenuProfile;
	%state = %client.BCM_MenuState;
	%menu = %client.BCM_MenuCache;
	%client.BCM_MenuPosition+= %dir;
	%position = %client.BCM_MenuPosition;
	%recordCount = getRecordCount(%menu);
	if(%position > %recordCount - 1 || %recordCount == 0)
	{
		%client.BCM_MenuPosition = %position = 0;
	}
	else if(%position < 0)
	{
		%client.BCM_MenuPosition = %position = %recordCount - 1;
	}
	if((%fn = $BCM::ScrollCallback[%profile, %state]) !$= "")
	{
		%selection = getRecord(%menu, %position);
		eval(%fn);
	}
	%client.BCM_MenuDisplay();
}

function GameConnection::BCM_MenuAddQuantity(%client, %amt)
{
	%profile = %client.BCM_MenuProfile;
	%state = %client.BCM_MenuState;
	%quantity = %client.BCM_MenuQuantity + %amt;
	if(%quantity < 0 && !$BCM::NegativeQuantity[%profile, %state])
	{
		%quantity = 0;
		messageClient(%client, 'MsgError');
	}
	%client.BCM_MenuQuantity = %quantity;
	if((%fn = $BCM::QuantityCallback[%profile, %state]) !$= "")
	{
		eval(%fn);
	}
	%client.BCM_MenuDisplay();
}

function GameConnection::BCM_MenuChangeState(%client, %newstate)
{
	%profile = %client.BCM_MenuProfile;
	%state = %client.BCM_MenuState;
	if(%state != %newstate)
	{
		%client.BCM_MenuPosition = 0;
	}
	%position = %client.BCM_MenuPosition;
	%client.BCM_MenuState = %state = %newstate;
	if((%quantity = $BCM::DefaultQuantity[%profile, %state]) !$= "")
	{
		%client.BCM_MenuQuantity = %quantity;
	}
	else
	{
		%client.BCM_MenuQuantity = 0;
	}
	%client.BCM_MenuCache = %menu = BCM_GetMenuText($BCM::Menu[%profile, %state], %client);
	if((%fn = $BCM::ScrollCallback[%profile, %state]) !$= "")
	{
		%selection = getRecord(%menu, %position);
		eval(%fn);
	}
	%client.BCM_MenuDisplay();
}

function GameConnection::BCM_MenuDisplay(%client)
{
	if(%client.BCM_MenuState == 0)
	{
		return %client.centerPrint();
	}
	%profile = %client.BCM_MenuProfile;
	%state = %client.BCM_MenuState;
	%menu = %client.BCM_MenuCache;
	%position = %client.BCM_MenuPosition;
	%recordCount = getRecordCount(%menu);
	%selection = getRecord(%menu, %position);
	if((%fn = $BCM::MenuFunction[%profile, %state]) !$= "")
	{
		return eval(%fn);
	}
	else if((%fn = $BCM::DisplayFunction[%profile, %state]) !$= "")
	{
		%print = eval(%fn);
	}
	else
	{
		%selection = hasColor(%selection) ? %selection : "\c2" @ %selection;
		if($BCM::EnableQuantity[%profile, %state])
		{
			%menu = setRecord(%menu, %position, "<div:1>\c3>>" @ %selection @ "\c3x" @ %client.BCM_MenuQuantity);
		}
		else
		{
			%menu = setRecord(%menu, %position, "<color:FFFF00><div:1>>>" @ %selection @ "<<");
		}
		%init = mClamp(%position - 2, 0, %recordCount - 5);
		%print = "\c2" @ getRecord(%menu, %init);
		for(%i = %init + 1; %i < %init + 5; %i++)
		{
			%rec = getRecord(%menu, %i);
			%rec = hasColor(%rec) ? %rec : "\c2" @ %rec;
			%print = %print NL %rec;
		}
	}
	%client.centerPrint(%print, 5);
	%client.scheduleBCM_MenuCancel();
}

function GameConnection::scheduleBCM_MenuCancel(%client)
{
	cancel(%client.BCM_MenuCancelSched);
	%client.BCM_MenuCancelSched = %client.schedule(5000, BCM_MenuCancel);
}