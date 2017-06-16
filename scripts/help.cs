//ReverieFortWars/crafting.cs

//TABLE OF CONTENTS

$RFW::Help[""] = "\c2Help topics list (use \c3/help topic name\c2): Menu, Getting started, Items, Crafting, Resources, Biomes, Building, Totems, Tribes, Forum thread";
$RFW::Help["Topics"] = $RFW::Help[""];

$RFW::Help["Menu"] = "\c2You can open the menu by pressing your \c3brick plant\c2 key (default: Enter).\n\c2You can navigate the menu with your \c3shift brick\c2 keys (default: numpad 2/4/6/8 or I/J/K/L).";

$RFW::Help["Getting started"] = "\c2Welcome to Reverie Fort Wars! First of all, you may have noticed the \c3Fist\c2 in your inventory.\t\c2You can use the Fist weapon to gather resources by breaking things.\t\c2Try gathering at least 32 wood, then use \c3/help Getting started 2\c2 to continue.";
$RFW::Help["Getting started 1"] = $RFW::Help["Getting started"];
$RFW::Help["Getting started 2"] = "\c2Now that you have some wood, you can make your first set of crafting tools.\n\c2Use \c3/craft Wooden crafting tools\c2 to make your crafting tools.\n\c2The crafting tools will be placed in your inventory, not in your tools.\n\c2Gather 48 Stone and 16 more Wood, then use \c3/help Getting started 3\c2 to continue.";
$RFW::Help["Getting started 3"] = "\c2Now you can upgrade your crafting tools to \c3Stone crafting tools\c2. Make those now.\t\c2Next, you should have enough resources left over to make a \c3Stone pickaxe\c2. Make one of those too.\n\c2The Stone pickaxe, like your crafting tools, was placed in your inventory.\n\c2Use \c3/equip Stone pickaxe\c2 to move it to your tools.\n\c2You can use \c3/unequip Stone pickaxe\c2 to return it to your inventory.\n\c2Use \c3/help Getting started 4\c2 when you're ready to continue.";
$RFW::Help["Getting started 4"] = "\c2Now, you can either start building a home or continue gathering resources.\t\c2Use \c3/help Building\c2 to learn about building, or \c3/help Getting started 5\c2 to learn about continued gathering.";
$RFW::Help["Getting started 5"] = "\c2Your Stone pickaxe is tougher than your fists, and allows you to gather better materials.\n\c3Surtra\c2 and \c3Obsidian\c2 are the next materials after Stone. Surtra can be found underground anywhere, but Obsidian is only found in the mountains.\t\c2Note that you cannot make a pickaxe from Obsidian, as it is too brittle, though you can make weapons and tools from it.\t\c2Make yourself a \c3Surtra pickaxe\c2 then use \c3/help Getting started 6\c2 to continue.";
$RFW::Help["Getting started 6"] = "\c2Next, you'll want to start gathering \c3Calcite\c2. Calcite is common in the desert and tundra biomes.\t\c3Sanguite\c2 follows Calcite. It, too, is found in the desert and tundra.\t\c2However, Sanguite is very rare outside of the dangerous flesh biomes. If you're feeling brave, seek one out and reap the rewards.";

$RFW::Help["Building"] = "\c2Building in Reverie Fort Wars is not all that different from normal building.\t\c2The main difference is that paint colors match up with resources. You must expend resources to place bricks. Larger bricks require more resources.\t\c2Other players can also destroy your bricks, so protect them! You can place a \c3Totem\c2 to protect your bricks.\t\c2Use \c3/help Totems\c2 to find out more.";
$RFW::Help["Totems"] = "\c3Totems\c2 allow you to lay claim to all bricks in a 200 stud radius of your totem.\t\c2Players who do not have trust with you cannot build within this radius, and take damage from hurting bricks in the radius.\t\c2Totems also grant certain effects to bricks, such as increasing or regenerating their health.\t\c2You can find the Totem brick under the Special tab.";

$RFW::Help["Items"] = "\c2Items can be crafted from \c3resources\c2. Use \c3/help crafting\c2 to learn more about crafting.";

$RFW::Help["Crafting"] = "\c2You can use \c3/craft item name\c2 to craft an item, or \c3/craft # item name\c2 to craft more than one of an item at a time.\t\c2Use \c3/help item name\c2 to find out how to craft a specific item.";

$RFW::Help["Resources"] = "\c3Resources\c2 are found throughout the world in brick form. A list of resources follows. Use \c3/help resource name\c2 to learn more about a resource.\t\c2Dirt, Wood, Plant matter, Sand, Snow, Stone, Cloth, Glass, Obsidian, Surtra, Calcite, Nithlite, Sanguite, Sinnite, Flesh";

$RFW::Help["Dirt"] = "\c3Dirt\c2 is found all around in grassy areas. It is weak and used only for building.";
$RFW::Help["Wood"] = "\c3Wood\c2 is found by cutting down trees. It is used in many tools and weapons as well as building.";
$RFW::Help["Plant matter"] = "\c3Plant matter\c2 is obtained from the green and colorful parts of plants.\t\c2It can be used to craft \c3Cloth\c2 or to reconstruct plants.";
$RFW::Help["Sand"] = "\c3Sand\c2 is found in desert areas. It can be used to make \c3Glass\c2.";
$RFW::Help["Snow"] = "\c3Snow\c2 is found in tundra areas. It can be used to make a \c3Snowball\c2 or to build a snowman.";
$RFW::Help["Stone"] = "\c3Stone\c2 is found underground throughout the world, but especially in mountains.\t\c2Stone can be used to craft primitive tools, and also makes an excellent building material.";
$RFW::Help["Cloth"] = "\c3Cloth\c2 is made from woven plant matter. It can be used to craft a \c3Cloth shirt\c2 or \c3Cloth pants\c2.\t\c2Cloth can also be used to build colorful decorations.";
$RFW::Help["Glass"] = "\c3Glass\c2 is a transparent material made from fired sand. It can be used to build windows.";
$RFW::Help["Obsidian"] = "\c3Obsidian\c2 is found in mountain areas. It is commonly used to make effective primitive weapons.";
$RFW::Help["Surtra"] = "\c3Sutra\c2 is a common metal found throughout the world. It is commonly used to make cheap weapons and tools.";
$RFW::Help["Calcite"] = "\c3Calcite\c2 is a metal found throughout the world, but most often in deserts and tundras.\t\c2Some alcehmists claim to be able to transmute bone into Calcite.\t\c2Calcite is often used to make good-quality weapons, armor, and tools.";
$RFW::Help["Nithlite"] = "\c3Nithlite\c2 is a synthetic metal crafted by the Photometallisynthesis Lens. It is used to make weapons and armor.";
$RFW::Help["Sanguite"] = "\c3Sanguite\c2 is a rare, high-quality metal most often found in deserts and tundras.\t\c2Some travelers say that the dangerous flesh biomes are filled with Sanguite\t\c2Alchemists sometimes claim to able to transmute Sanguite from blood.\t\c2Sanguite is used to make high-quality weapons, armor, and tools.";
$RFW::Help["Sinnite"] = "\c3Sinnite\c2 is a high-quality synthetic metal crafted by the Photometallisynthesis Lens.\t\c2It is used to make weapons and armor.";
$RFW::Help["Flesh"] = "\c3Flesh\c2 is the material that makes up animals and humans.\t\c2There exist flesh biomes, which are filled with outlandish, dangerous creatures.";

$RFW::Help["Biomes"] = "\c2Biome list: grassy, desert, tundra, mountain, flesh";

$RFW::Help["Tribes"] = "\c3Tribes\c2 are blood alliances holding together the people of the Reverie.\t\c2Work together with your fellow tribesmen to assert your dominance over the enemy tribe.\t\c2You can switch to another tribe with \c5/switchTribe name\c2, but you will lose all of your bricks and items.";

// $RFW::Help["Item list"] = "\c2Wooden crafting tools, Wooden sword, Wooden armor, Wooden bow, Wooden arrow, Cloth shirt, Cloth pants\t\c2Stone crafting tools, Stone pickaxe, Stone sword, Stone dagger, Stone armor, Stone arrow\t\c2Obsidian crafting tools, Obsidian sword, Obsidian dagger, Obsidian arrow";
// $RFW::Help["Items list"] = $RFW::Help["Item list"];

$RFW::Help["Thread"] = "<a:forum.blockland.us/index.php?topic=281292.0>Reverie Fort Wars thread</a>";
$RFW::Help["Forum thread"] = $RFW::Help["Thread"];

function GameConnection::multiMessage(%client, %msg, %delay)
{
	cancel(%client.multiMessageSched);
	messageClient(%client, '', getField(%msg, 0));
	if(getFieldCount(%msg) == 1)
	{
		return;
	}
	%client.multiMessageSched = %client.schedule(%delay, multiMessage, getFields(%msg, 1, getFieldCount(%msg)), %delay);
}

function serverCmdHelp(%client, %w1, %w2, %w3, %w4, %w5)
{
	%w = trim(%w1 SPC %w2 SPC %w3 SPC %w4 SPC %w5);
	if((%msg = $RFW::Help[%w]) !$= "")
	{
		%client.multiMessage(%msg, 3500);
	}
	else
	{
		messageClient(%client, 'MsgError', "<color:FF0000>Help topic for \"" @ %w @ "\" not found. Try <spush>\c3/help topics<spop>.");
	}
}