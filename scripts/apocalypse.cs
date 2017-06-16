//RFW Apocalypse
function RFW_Apocalypse()
{
	$RFW::Apocalypse = true;
	for(%i = 0; %i < clientGroup.getCount(); %i++)
	{
		%client = clientGroup.getObject(%i);
		%client.centerPrintQueue("<color:FFD800>BEHOLD THE APOCALYPSE!", 60000);
		%cam = %client.camera;
		%client.setControlObject(%cam);
		if(isObject(%obj = %client.player))
		{
			%obj = %client.player;
			%cam.setTransform(%obj.getTransform());
			%obj.delete();
		}
	}
	//clear all bricks
	for(%i = 0; %i < mainBrickGroup.getCount(); %i++)
	{
		mainBrickGroup.getObject(%i).chainDeleteAll();
	}
	schedule(1000, 0, exec, "Add-Ons/Gamemode_ReverieFortWars/scripts/buildWorld.cs");
	//schedule(1000, 0, ServerDirectSaveFileLoad, "saves/Beta City.bls", 3, "", 2);
}

package RFW_Apocalypse
{
	function GameConnection::SpawnPlayer(%client)
	{
		if(!$RFW::Apocalypse)
		{
			return parent::SpawnPlayer(%client);
		}
	}

	function ServerLoadSaveFile_End()
	{
		$RFW::Apocalypse = false;
		parent::ServerLoadSaveFile_End();
		for(%i = 0; %i < clientGroup.getCount(); %i++)
		{
			clientGroup.getObject(%i).spawnPlayer();
		}
	}
};
activatePackage(RFW_Apocalypse);