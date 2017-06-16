//ReverieFortWars/notice.cs

//TABLE OF CONTENTS
//#1. functions
//	#1.1 noticeTick
//#2. registrations
//	#2.1 notices

//#1.
//	#1.1
function noticeTick(%delay)
{
	cancel($noticeTick);
	while((%r = getRandom(1, $noticeCount)) == $lastNotice)
	{
	}
	messageAll('', "\c7bot\c4TipsBot\c6: " @ $Notice[%r]);
	$lastNotice = %r;
	$noticetick = schedule(%delay, 0, noticeTick, %delay);
}
schedule(90000, 0, noticeTick, 90000);

//#2.
//	#2.1
$NoticeCount = 0;
$Notice[$NoticeCount++] = "You can use your building controls to manipulate the menu. (\c5/help menu\c6)";
$Notice[$NoticeCount++] = "Faeries are dangerous, but killing them can allow you to collect valuable faerie dust.";
$Notice[$NoticeCount++] = "Sanguite is most often found underground in a flesh biome.";
$Notice[$NoticeCount++] = "Calcite is most often found underground in a flesh biome.";
$Notice[$NoticeCount++] = "Surtra is most often found underground in a mountain biome.";
$Notice[$NoticeCount++] = "Mountains (especially underground) are the only place you can find Obsidian.";
$Notice[$NoticeCount++] = "Sanguite can be transmuted from blood if you have a sanguimetallisynthesis wand.";
$Notice[$NoticeCount++] = "Calcite can be transmuted from bone if you have an ossimetallisynthesis totem.";
$Notice[$NoticeCount++] = "Check out the <a:forum.blockland.us/index.php?topic=281292.0>forum thread</a>\c6! Be sure to vote in the poll.";
$Notice[$NoticeCount++] = "The Malka tribe is a blood cult that worships Shessim, the Lord of Slaughter.";
$Notice[$NoticeCount++] = "The Prelkan tribe is a flesh cult that worships the Fabric, an entity binding all things together.";
$Notice[$NoticeCount++] = "The Kazezi tribe of lightweavers worship Naramata, the god of light and life.";
$Notice[$NoticeCount++] = "The forest-dwelling Faeborn tribe worships faeries and other woodland spirits.";
$Notice[$NoticeCount++] = "The desert-dwelling Sarti tribe serves the Darkins, a malicious, shadowy race.";
$Notice[$NoticeCount++] = "The deep-dwelling Eristia tribe worships kobolds and other earth spirits.";
$Notice[$NoticeCount++] = "The Munaa tribe is a fire cult that worships the mountain-dwelling Giants.";
$Notice[$NoticeCount++] = "The Mediir tribe is a death cult that worships Rimuto, god of death and bones.";
$Notice[$NoticeCount++] = "You can loot a corpse by activating it.";
$Notice[$NoticeCount++] = "You can butcher a corpse for bone, flesh, and blood by attacking it with a melee weapon.";
$Notice[$NoticeCount++] = "When you die, you lose up to 3 random items (or 20% of a stack of an item) from your inventory.";
$Notice[$NoticeCount++] = "You can place a spawn point brick to set the spot at which you will return to life.";
$Notice[$NoticeCount++] = "You can place a totem brick to claim an area, protecting your build. (\c5/help totems\c6)";
$Notice[$NoticeCount++] = "You can unequip an item with the \c5/unequip item name\c6 command.";
$Notice[$NoticeCount++] = "You can switch tribes with \c5/switchTribe tribe\c6 command, but will lose all of your items and bricks.";
$Notice[$NoticeCount++] = "When mining, remember that ores spawn in clusters.";
$Notice[$NoticeCount++] = "The Faerie Wand allows you to add lights and emitters to bricks.";
$Notice[$NoticeCount++] = "Have you tried Lawry's Seasoned Salt on the flesh of your enemies?";