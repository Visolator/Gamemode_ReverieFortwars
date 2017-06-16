//ReverieFortWars/accounts.cs

//TABLE OF CONTENTS
// #1. documentation
// #2. item system functionality
//   #2.1 readability/helper functions
// #3. item enumeration

// #1.
//////////////////
// ITEM STRINGS //
//////////////////
// Items are stored as strings. Data stored in these strings are seperated by tabs - each datum occupies a field.
// Field 0: Item type (an int, the item's type index, see following item data documentation)
// Field 1: Item kills (for weapons) / quantity (for stackable items) / 0 (for all other items)
// Field 2: Item name (for named weapons) / 0 (for all other items)
///////////////
// ITEM DATA //
///////////////
// $RFW::ItemDataCount      | int count of the number of defined item data
// $RFW::Item[i]            | string name of the ith item data
// $RFW::ItemWearable[str]  | bool whether the item named str can be worn (eg as armor)
// $RFW::ItemSlot[str]      | string the slot the item goes into, if wearable (eg armor, back...)
// $RFW::ItemEffect[str]    | string eval'd when the item is used from inventory (unwearable, unequipable items only)
// $RFW::ItemEquipable[str] | bool whether the item named str can be equipped (eg as a weapon)
// $RFW::ItemDatablock[str] | datablock (itemData) the item datablock corresponding to the item named str
// $RFW::ItemDrops[str]     | bool whether the item can drop on death
// $RFW::Item[str]          | int the index of the item datum with the provided name


// #2.

// #2.1
function getItemType(%str)
{
	return getField(%str, 0);
}

function getItemQuantity(%str)
{
	return getField(%str, 1);
}
function setItemQuantity(%str, %amt)
{
	return setField(%str, 1, %amt);
}
function incItemQuantity(%str, %amt)
{
	return setField(%str, 1, mFloatLength(getField(%str, 1) + %amt, 2));
}

function getItemKills(%str)
{
	return getField(%str, 1);
}
function setItemKills(%str, %amt)
{
	return setField(%str, 1, %amt);
}
function incItemKills(%str, %amt)
{
	return setField(%str, 1, mFloatLength(getField(%str, 1) + %amt, 0));
}

function getItemName(%str)
{
	return getField(%str, 2);
}
function setItemName(%str, %newName)
{
	return setField(%str, 2, %newName);
}

//
$RFW::ItemDataCount = 0;

//registers an item
function RFW_RegisterItem(%name, %stackable, %wearable, %equipable, %datablock, %slot, %canDrop)
{
	%x = $RFW::ItemDataCount++;
	$RFW::Item[%x] = %name;
	$RFW::ItemStackable[%name] = %stackable;
	$RFW::ItemWearable[%name] = %wearable;
	$RFW::ItemEquipable[%name] = %equipable;
	if(!%wearable && !%equipable && strLen(%slot))
	{
		$RFW::ItemEffect[%name] = %slot;
	}
	else
	{
		$RFW::ItemSlot[%name] = %slot;
	}
	$RFW::ItemDrops[%name] = %canDrop;
	if(%datablock !$= "0")
	{
		$RFW::ItemDatablock[%name] = %datablock.getID();
	}
	$RFW::Item[%name] = %x;
}

// #3.

RFW_RegisterItem("Fists", false, false, true, FistsItem, "", false);
//materials
RFW_RegisterItem("Dirt", true, false, false, 0, "", true);
RFW_RegisterItem("Wood", true, false, false, 0, "", true);
RFW_RegisterItem("Sand", true, false, false, 0, "", true);
RFW_RegisterItem("Stone", true, false, false, 0, "", true);
RFW_RegisterItem("Plant matter", true, false, false, 0, "", true);
RFW_RegisterItem("Snow", true, false, false, 0, "", true);
RFW_RegisterItem("Cloth", true, false, false, 0, "", true);
RFW_RegisterItem("Paper", true, false, false, 0, "", true);
RFW_RegisterItem("Leather", true, false, false, 0, "", true);
RFW_RegisterItem("Obsidian", true, false, false, 0, "", true);
RFW_RegisterItem("Glass", true, false, false, 0, "", true);
RFW_RegisterItem("Bone", true, false, false, 0, "", true);
RFW_RegisterItem("Blood", true, false, false, 0, "", true);
RFW_RegisterItem("Flesh", true, false, false, 0, "", true);
RFW_RegisterItem("Faerie dust", true, false, false, 0, "", true);
RFW_RegisterItem("Surtra", true, false, false, 0, "", true);
RFW_RegisterItem("Calcite", true, false, false, 0, "", true);
RFW_RegisterItem("Nithlite", true, false, false, 0, "", true);
RFW_RegisterItem("Dasnia", true, false, false, 0, "", true);
RFW_RegisterItem("Sanguite", true, false, false, 0, "", true);
RFW_RegisterItem("Sinnite", true, false, false, 0, "", true);
RFW_RegisterItem("Crystal", true, false, false, 0, "", true);
//tools
RFW_RegisterItem("Wooden crafting tools", false, false, false, 0, "", false);
RFW_RegisterItem("Faerie crafting tools", false, false, false, 0, "", false);
RFW_RegisterItem("Faerie wand", false, false, true, FaerieWandItem, "", true);
RFW_RegisterItem("Stone crafting tools", false, false, false, 0, "", false);
RFW_RegisterItem("Obsidian crafting tools", false, false, false, 0, "", false);
RFW_RegisterItem("Surtra crafting tools", false, false, false, 0, "", false);
RFW_RegisterItem("Calcite crafting tools", false, false, false, 0, "", false);
RFW_RegisterItem("Nithlite crafting tools", false, false, false, 0, "", false);
RFW_RegisterItem("Sanguite crafting tools", false, false, false, 0, "", false);
RFW_RegisterItem("Sinnite crafting tools", false, false, false, 0, "", false);
RFW_RegisterItem("Pyromancy wand", false, false, true, PyroWandItem, "", true);
RFW_RegisterItem("Alchemy vials", false, false, false, 0, "", true);
RFW_RegisterItem("Photometallisynthesis lens", false, false, true, PhotoLensItem, "", true);
RFW_RegisterItem("Ossimetallisynthesis staff", false, false, true, OssiStaffItem, "", true);
RFW_RegisterItem("Sanguimetallisynthesis wand", false, false, true, SanguiWandItem, "", true);
//equipment
RFW_RegisterItem("Snowball", true, false, true, SnowballItem, "", true);

RFW_RegisterItem("Wooden sword", false, false, true, WoodenSwordItem, "", true);
RFW_RegisterItem("Wooden bow", false, false, true, WoodenBowItem, "", true);
RFW_RegisterItem("Wooden quiver", false, true, false, 0, "Back", true);
RFW_RegisterItem("Wooden armor", false, true, false, 0, "Armor", true);

RFW_RegisterItem("Bone sword", false, false, true, BoneSwordItem, "", true);
RFW_RegisterItem("Bone bow", false, false, true, BoneBowItem, "", true);
RFW_RegisterItem("Bone quiver", false, true, false, 0, "Back", true);
RFW_RegisterItem("Bone armor", false, true, false, 0, "Armor", true);

RFW_RegisterItem("Faerie bow", false, false, true, FaerieBowItem, "", true);
RFW_RegisterItem("Faerie armor", false, true, false, 0, "Armor", true);

RFW_RegisterItem("Stone sword", false, false, true, StoneSwordItem, "", true);
RFW_RegisterItem("Stone quiver", false, true, false, 0, "Back", true);
RFW_RegisterItem("Stone pickaxe", false, false, true, StonePickaxeItem, "", true);
RFW_RegisterItem("Stone armor", false, true, false, 0, "Armor", true);

RFW_RegisterItem("Obsidian sword", false, false, true, macahuitlItem, "", true);
RFW_RegisterItem("Obsidian dagger", false, false, true, ObsidianDaggerItem, "", true);
RFW_RegisterItem("Obsidian quiver", false, true, false, 0, "Back", true);

RFW_RegisterItem("Surtra sword", false, false, true, SurtraSwordItem, "", true);
RFW_RegisterItem("Surtra dagger", false, false, true, SurtraDaggerItem, "", true);
RFW_RegisterItem("Surtra pickaxe", false, false, true, SurtraPickaxeItem, "", true);
RFW_RegisterItem("Surtra quiver", false, true, false, 0, "Back", true);
RFW_RegisterItem("Surtra armor", false, true, false, 0, "Armor", true);

RFW_RegisterItem("Calcite sword", false, false, true, CalciteSwordItem, "", true);
RFW_RegisterItem("Calcite dagger", false, false, true, CalciteDaggerItem, "", true);
RFW_RegisterItem("Calcite pickaxe", false, false, true, CalcitePickaxeItem, "", true);
RFW_RegisterItem("Calcite quiver", false, true, false, 0, "Back", true);
RFW_RegisterItem("Calcite armor", false, true, false, 0, "Armor", true);

RFW_RegisterItem("Nithlite sword", false, false, true, NithliteSwordItem, "", true);
RFW_RegisterItem("Nithlite dagger", false, false, true, NithliteDaggerItem, "", true);
RFW_RegisterItem("Nithlite pickaxe", false, false, true, NithlitePickaxeItem, "", true);
RFW_RegisterItem("Nithlite quiver", false, true, false, 0, "Back", true);
RFW_RegisterItem("Nithlite armor", false, true, false, 0, "Armor", true);

RFW_RegisterItem("Sanguite sword", false, false, true, SanguiteSwordItem, "", true);
RFW_RegisterItem("Sanguite dagger", false, false, true, SanguiteDaggerItem, "", true);
RFW_RegisterItem("Sanguite pickaxe", false, false, true, SanguitePickaxeItem, "", true);
RFW_RegisterItem("Sanguite quiver", false, true, false, 0, "Back", true);
RFW_RegisterItem("Sanguite armor", false, true, false, 0, "Armor", true);

RFW_RegisterItem("Sinnite sword", false, false, true, SinniteSwordItem, "", true);
RFW_RegisterItem("Sinnite dagger", false, false, true, SinniteDaggerItem, "", true);
RFW_RegisterItem("Sinnite pickaxe", false, false, true, SinnitePickaxeItem, "", true);
RFW_RegisterItem("Sinnite quiver", false, true, false, 0, "Back", true);
RFW_RegisterItem("Sinnite armor", false, true, false, 0, "Armor", true);

RFW_RegisterItem("Crystal sword", false, false, true, CrystalSwordItem, "", true);
RFW_RegisterItem("Crystal dagger", false, false, true, CrystalDaggerItem, "", true);
RFW_RegisterItem("Crystal pickaxe", false, false, true, CrystalPickaxeItem, "", true);
RFW_RegisterItem("Crystal quiver", false, true, false, 0, "Back", true);
RFW_RegisterItem("Crystal armor", false, true, false, 0, "Armor", true);
RFW_RegisterItem("Crystal bow", false, false, true, CrystalBowItem, "", true);
//clothes
RFW_RegisterItem("Cloth shirt", false, true, false, 0, "Shirt", true);
RFW_RegisterItem("Cloth pants", false, true, false, 0, "Pants", true);
RFW_RegisterItem("Cloth hat", false, true, false, 0, "Hat", true);
RFW_RegisterItem("Cloth cape", false, true, false, 0, "Back", true);
RFW_RegisterItem("Cloth robe", false, true, false, 0, "Shirt", true);
RFW_RegisterItem("Leather boots", false, true, false, 0, "Feet", true);
RFW_RegisterItem("Leather gloves", false, true, false, 0, "Hands", true);
//medicines and potions
RFW_RegisterItem("Cloth bandage", true, false, false, 0, "%player.bandage();", true);
RFW_RegisterItem("Pishelva", true, false, false, 0, "%player.pishelva();", true);
RFW_RegisterItem("Hoborchakin", true, false, false, 0, "%player.hoborchakin();", true);
RFW_RegisterItem("Vorpal potion", true, false, false, 0, "%player.vorpalPotion();", true);
RFW_RegisterItem("Fleshweaver's potion", true, false, false, 0, "%player.fleshweaverPotion();", true);
RFW_RegisterItem("Crimson potion", true, false, true, 0, "", true);

//potion functionality
function Player::Bandage(%this)
{
	%time = getSimTime();
	if(%this.bandageTime + 60000 < %time)
	{
		%client = %this.client;
		messageClient(%client, 'MsgError');
		%client.centerPrintQueue("<color:FF0000>Can't use that yet.", 3);
		return;
	}
	%this.client.account.removeItem("Cloth bandage\t1\t", true);
	%this.bandageTime = %time;
	for(%i = 0; %i < 5; %i++)
	{
		%this.schedule(%i * 3000, addHealth, 10);
	}
}

function Player::Hoborchakin(%this)
{
	%this.client.account.removeItem("Hoborchakin\t1\t", true);
	cancel(%this.scaleSched);
	%time = getSimTime();
	%scale = %this.getScale();
	%this.startScale = %scale;
	%this.growVec = vectorSub("1.5 1.5 1.5", %scale);
	%this.hoborchakinTime = %time;
	%this.hoborchakin1Time = %time + 5000;
	%this.hoborchakin2Time = %time + 305000;
	%this.hoborchakin1();
}

function Player::Hoborchakin1(%this)
{
	cancel(%this.scaleSched);
	if(!isObject(%this) || %this.getState() $= "Dead")
	{
		return;
	}
	%time = getSimTime();
	%endTime = %this.hoborchakin1Time;
	if(%time > %endTime)
	{
		%this.setScale("1.5 1.5 1.5");
		%this.growVec = "-0.5 -0.5 -0.5";
		%this.scaleSched = %this.schedule(50, Hoborchakin2);
		return;
	}
	%timePct = 1 - (%endTime - %time) / 5000;
	%scale = vectorAdd(%this.startScale, vectorScale(%this.growVec, %timePct));
	%this.setScale(%scale);
	%this.scaleSched = %this.schedule(50, Hoborchakin1);
}

function Player::Hoborchakin2(%this)
{
	cancel(%this.scaleSched);
	if(!isObject(%this) || %this.getState() $= "Dead")
	{
		return;
	}
	%time = getSimTime();
	%endTime = %this.hoborchakin2Time;
	if(%time > %endTime)
	{
		%this.setScale("1 1 1");
		return;
	}
	%timePct = 1 - (%endTime - %time) / 300000;
	%scale = vectorAdd("1.5 1.5 1.5", vectorScale("-0.5 -0.5 -0.5", %timePct));
	%this.setScale(%scale);
	%this.scaleSched = %this.schedule(50, Hoborchakin2);
}

function Player::Pishelva(%this)
{
	%this.client.account.removeItem("Pishelva\t1\t", true);
	cancel(%this.scaleSched);
	%time = getSimTime();
	%scale = %this.getScale();
	%this.startScale = %scale;
	%this.growVec = vectorSub("0.5 0.5 0.5", %scale);
	%this.PishelvaTime = %time;
	%this.Pishelva1Time = %time + 5000;
	%this.Pishelva2Time = %time + 305000;
	%this.Pishelva1();
}

function Player::Pishelva1(%this)
{
	cancel(%this.scaleSched);
	if(!isObject(%this) || %this.getState() $= "Dead")
	{
		return;
	}
	%time = getSimTime();
	%endTime = %this.Pishelva1Time;
	if(%time > %endTime)
	{
		%this.setScale("0.5 0.5 0.5");
		%this.growVec = "0.5 0.5 0.5";
		%this.scaleSched = %this.schedule(50, Pishelva2);
		return;
	}
	%timePct = 1 - (%endTime - %time) / 5000;
	%scale = vectorAdd(%this.startScale, vectorScale(%this.growVec, %timePct));
	%this.setScale(%scale);
	%this.scaleSched = %this.schedule(50, Pishelva1);
}

function Player::Pishelva2(%this)
{
	cancel(%this.scaleSched);
	if(!isObject(%this) || %this.getState() $= "Dead")
	{
		return;
	}
	%time = getSimTime();
	%endTime = %this.Pishelva2Time;
	if(%time > %endTime)
	{
		%this.setScale("1 1 1");
		return;
	}
	%timePct = 1 - (%endTime - %time) / 305000;
	%scale = vectorAdd("0.5 0.5 0.5", vectorScale("0.5 0.5 0.5", %timePct));
	%this.setScale(%scale);
	%this.scaleSched = %this.schedule(50, Pishelva2);
}