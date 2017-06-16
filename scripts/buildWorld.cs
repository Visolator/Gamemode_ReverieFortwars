//ReverieFortWars/buildWorld.cs

//exec("Add-Ons/Gamemode_ReverieFortWars/scripts/buildWorld.cs");

if($RFW::Resources["Dirt"] $= "")
{
	exec("Add-Ons/Gamemode_ReverieFortWars/scripts/resources.cs");
}
exec("Add-Ons/Gamemode_ReverieFortWars/scripts/doodads.cs");

//TABLE OF CONTENTS
// #1. Parameters
// #2. High-level functions
//   #2.1 ReverieFortWars_BuildWorld
//   #2.2 ReverieFortWars_BeginPlanWorld
//   #2.3 ReverieFortWars_DiamondSquare
//   #2.4 ReverieFortWars_AddBiomes
//   #2.5 ReverieFortWars_ConstructWorld
//   #2.6 ReverieFortWars_HydraulicErosion
// #3. Low-level functions

// #1.

$RFW::WorldBasis = nameToID(brick16xCubeData);
$RFW::WorldBasisName = $RFW::WorldBasis.uiName;
//$RFW::WorldBasisGrass = nameToID(brick16);

$RFW::WorldSize = 256; //1, 2, 4, 8, 16, 32, 64, 128, 256, or 512
$RFW::WorldPlanSpeed = 250; //plannings per millisecond
$RFW::WorldBuildSpeed = 250; //bricks written to file per millisecond
$RFW::WorldSnapZ = false; //snap bricks to a Z-grid (a la normal cubescaping)

$RFW::WorldBaseHeight = 256;
$RFW::WorldBumpiness = 40 * $RFW::WorldSize / 512;

$RFW::WorldMountainHeight = $RFW::WorldBaseHeight + 25;

//$RFW::WorldForests = 0;
//$RFW::WorldForestMinRadius = 100;
//$RFW::WorldForestMaxRadius = 150;

$RFW::WorldDeserts = 2;
$RFW::WorldDesertMinRadius = 100 * $RFW::WorldSize / 512;
$RFW::WorldDesertMaxRadius = 150 * $RFW::WorldSize / 512;

$RFW::WorldTundras = 1;
$RFW::WorldTundraMinRadius = 100 * $RFW::WorldSize / 512;
$RFW::WorldTundraMaxRadius = 150 * $RFW::WorldSize / 512;

$RFW::WorldFleshBiomes = 3;
$RFW::WorldFleshMinRadius = 10;
$RFW::WorldFleshMaxRadius = 14;

$RFW::WorldDoodadFrequency = 0.0625; // 1/16

$RFW::WorldBasisSizeX = $RFW::WorldBasis.brickSizeX * 0.5;
$RFW::WorldBasisSizeY = $RFW::WorldBasis.brickSizeY * 0.5;
$RFW::WorldBasisSizeZActual = $RFW::WorldBasis.brickSizeZ * 0.2;
$RFW::WorldBasisSizeZ = $RFW::WorldSnapZ ? $RFW::WorldBasisSizeZActual : 0.6;

$RFW::Erosion::Cycles = 0;
$RFW::Erosion::RainAmt = 20; //base additional water per tile per iteration
$RFW::Erosion::Solubility = 10; //soil lost per tile per iteration
$RFW::Erosion::Evaporation = 25; //maximum water evaporation per tile per iteration
$RFW::Erosion::Capacity = 0.5; //max sediment per unit water


//$RFW::World::Z

function min(%a, %b)
{
	return %a < %b ? %a : %b;
}

// #2.
// #2.1
function ReverieFortWars_BuildWorld()
{
	echo("RFW: Building world...");
	
	//setup
	if(isObject(groundPlane))
	{
		//groundPlane.delete();
	}
	cancel($WorldBuildSched);
	cancel($WorldPlanSched);
	deleteVariables("RFW::Tunneled*");
	deleteVariables("RFW::World*");

	//ready the file
	if($AddOn__Support_AutoSaver != 1)
	{
		$RFW::FileObject = new FileObject(RFW_FileObject){};
		MissionCleanup.add($RFW::FileObject);
		$RFW::FileObject.openForWrite("saves/RFW.bls");
		$RFW::FileObject.writeLine("This is a Blockland save file.  You probably shouldn't modify it cause you'll screw it up.");
		$RFW::FileObject.writeLine("1");
		$RFW::FileObject.writeLine("");
		for(%i = 0; %i < 64; %i++)
		{
			$RFW::FileObject.writeLine(getColorIdTable(%i));
		}
		$RFW::FileObject.writeLine("Linecount 151"); //pretty sure this doesn't do anything
	}

	//initialize diamond-square
	$RFW::World::Z[0, 0] = $RFW::WorldBaseHeight - 25;
	$RFW::World::Z[$RFW::WorldSize, 0] = $RFW::WorldBaseHeight + 25;
	$RFW::World::Z[0, $RFW::WorldSize] = $RFW::WorldBaseHeight - 25;
	$RFW::World::Z[$RFW::WorldSize, $RFW::WorldSize] = $RFW::WorldBaseHeight + 25;
	ReverieFortWars_DiamondSquare(0, 0, $RFW::WorldSize, $RFW::WorldSize);

	schedule(10, 0, ReverieFortWars_AddBiomes);
}

// #2.2
function ReverieFortWars_BeginPlanWorld(%x, %y)
{
	for(%i = 0; %i < $RFW::WorldPlanSpeed; %i++)
	{
		%xContrib = 25 * mCos(%x / 23 + 7) + 20 * mCos(%x / 47 + 12) + 15 * mCos(%x / 27 + 58) + 10 * mCos(%x / 61 + 30) + 5 * mCos(%x / 23 + 2);
		%yContrib = 25 * mCos(%x / 59 + 89) + 20 * mCos(%y / 21 + 7) + 15 * mCos(%y / 91 + 4) + 10 * mCos(%y / 19 + 8) + 5 * mCos(%y / 37 + 17);
		$RFW::World::Z[%x, %y] = 100 + %xContrib + %yContrib;
		//iterate
		if(%y >= $RFW::WorldSize)
		{
			%x++;
			if(%x >= $RFW::WorldSize)
			{
				echo("RFW: Shape initialization complete.");
				//ReverieFortWars_PlanWorld(0);
				ReverieFortWars_AddBiomes();
				return;
			}
			%y = 0;
		}
		else
		{
			%y++;
		}
	}
	$WorldPlanSched = schedule(1, 0, ReverieFortWars_BeginPlanWorld, %x, %y);
}

// #2.3

function ReverieFortWars_DiamondSquare(%minX, %minY, %maxX, %maxY)
{
	%midX = (%minX + %maxX) / 2;
	%midY = (%minY + %maxY) / 2;
	%difference = %midX - %minX;
	%magnitude = $RFW::WorldBumpiness * 2 * %difference / $RFW::WorldSize;

	//diamond step
	%cornerAvg = ($RFW::World::Z[%minX, %minY] + $RFW::World::Z[%maxX, %minY] + $RFW::World::Z[%minX, %maxY] + $RFW::World::Z[%maxX, %maxY]) / 4;
	%mid = %cornerAvg + %magnitude * $RFW::WorldBumpiness * (getRandom() - 0.5);
	$RFW::World::Z[%midX, %midY] = %mid;
	//square step
	%mid = %cornerAvg + %magnitude * $RFW::WorldBumpiness * (getRandom() - 0.5);
	$RFW::World::Z[%minX, %midY] = %mid;
	%mid = %cornerAvg + %magnitude * $RFW::WorldBumpiness * (getRandom() - 0.5);
	$RFW::World::Z[%midX, %minY] = %mid;
	%mid = %cornerAvg + %magnitude * $RFW::WorldBumpiness * (getRandom() - 0.5);
	$RFW::World::Z[%midX, %maxY] = %mid;
	%mid = %cornerAvg + %magnitude * $RFW::WorldBumpiness * (getRandom() - 0.5);
	$RFW::World::Z[%maxX, %midY] = %mid;
	//check if we're done
	if(%difference <= 1)
	{
		return;
	}
	//recurse
	schedule(1, 0, ReverieFortWars_DiamondSquare, %minX, %minY, %midX, %midY);
	schedule(2, 0, ReverieFortWars_DiamondSquare, %midX, %minY, %maxX, %midY);
	schedule(3, 0, ReverieFortWars_DiamondSquare, %minX, %midY, %midX, %maxY);
	$WorldPlanSched = schedule(4, 0, ReverieFortWars_DiamondSquare, %midX, %midY, %maxX, %maxY);
}


// #2.4
function ReverieFortWars_AddBiomes()
{
	for(%tundras = 0; %tundras < $RFW::WorldTundras; %tundras++)
	{
		RFW_AddBiome(getRandom(0, $RFW::WorldSize), getRandom($RFW::WorldSize / 2, $RFW::WorldSize), getRandom($RFW::WorldTundraMinRadius, $RFW::WorldTundraMaxRadius), $RFW::Resource["Snow"]);
	}
	for(%deserts = 0; %deserts < $RFW::WorldDeserts; %deserts++)
	{
		RFW_AddBiome(getRandom(0, $RFW::WorldSize), getRandom(0, $RFW::WorldSize / 2), getRandom($RFW::WorldDesertMinRadius, $RFW::WorldDesertMaxRadius), $RFW::Resource["Sand"]);
	}
	for(%fleshbiomes = 0; %fleshbiomes < $RFW::WorldTundras; %fleshbiomes++)
	{
		RFW_AddBiome(getRandom(0, $RFW::WorldSize), getRandom(0, $RFW::WorldSize), getRandom($RFW::WorldFleshMinRadius, $RFW::WorldFleshMaxRadius), $RFW::Resource["Flesh"]);
	}
	echo("RFW: Biome initialization complete.");
	//ReverieFortWars_ConstructWorld(0, 0);
	ReverieFortWars_HydraulicErosion(0);
}

// #2.5
function ReverieFortWars_ConstructWorld(%x, %y)
{
	if(isEventPending($WorldPlanSched))
	{
		$WorldBuildSched = schedule(10, 0, ReverieFortWars_ConstructWorld, %x, %y);
		return;
	}
	if(%x == 0 && %y == 0)
	{
		echo("RFW: Saving world to file ...");
	}
	for(%i = 0; %i < $RFW::WorldBuildSpeed; %i++)
	{
		%z = mFloatLength($RFW::World::Z[%x, %y] / $RFW::WorldBasisSizeZ, 0) * $RFW::WorldBasisSizeZ;
		%pos = (%x * $RFW::WorldBasisSizeX) SPC (%y * $RFW::WorldBasisSizeY) SPC %z;
		//plant the brick
		%m = $RFW::World::M[%x, %y];
		if(%m $= "")
		{
			if(%z > $RFW::WorldMountainHeight)
			{
				%m = $RFW::Resource["Stone"];
			}
			else
			{
				%m = $RFW::Resource["Dirt"];
			}
		}
		//data" x y z angleid ?(1) color print(leave blank for non print bricks) colorfx? shapefx? raycasting? colliding? rendering?
		if($AddOn__Support_AutoSaver != 1)
			$RFW::FileObject.writeLine($RFW::WorldBasisName @ "\" " @ %pos SPC 0 SPC 1 SPC $RFW::ResourceColor[%m] @ "  " @ $RFW::ResourceColorFx[%m] SPC 0 SPC 1 SPC 1 SPC 1);
		if(%m == $RFW::Resource["Dirt"])
		{
			//add grass on top
			%plantMatter = $RFW::Resource["Plant matter"];
			//$RFW::FileObject.writeLine("4x4F\" " @ setWord(%pos, 2, %z + 1.1) SPC 0 SPC 1 SPC $RFW::ResourceColor[%plantMatter] @ "  " @ $RFW::ResourceColorFx[%plantMatter] SPC 0 SPC 1 SPC 1 SPC 1);
			%doodadOffset = vectorAdd(%pos, "0 0 4.1");
			if($AddOn__Support_AutoSaver != 1)
				$RFW::FileObject.writeLine("16x16 Base\" " @ %doodadOffset SPC 0 SPC 1 SPC $RFW::ResourceColor[%plantMatter] @ "  " @ $RFW::ResourceColorFx[%plantMatter] SPC 0 SPC 1 SPC 1 SPC 1);
		}
		else
		{
			%doodadOffset = %pos;
		}
		if(getRandom() < $RFW::WorldDoodadFrequency)
		{
			%matName = $RFW::Resource[%m];
			if($Doodads[%matName] > 0)
			{
				%r = getRandom(0, $Doodads[%matName]);
				%doodad = $Doodad[%matName, %r];
				if(getWordCount(%doodad) == 1)
				{
					for(%l = 0; %l < %doodad; %l++)
					{
						%subdoodad = $Doodad[%matname, %r, %l];
						%lines = getRecordCount(%subdoodad);
						for(%m = 0; %m < %lines; %m++)
						{
							%line = getRecord(%subdoodad, %m);
							%quotPos = strPos(%line, "\"") + 2;
							%brickName = getSubStr(%line, 0, %quotPos);
							%restLine = getSubStr(%line, %quotPos, strLen(%line));
							%restLine = vectorAdd(%doodadOffset, getWords(%restLine, 0, 2)) SPC getWords(%restLine, 3, 11);
							if($AddOn__Support_AutoSaver != 1)
								$RFW::FileObject.writeLine(%brickName @ %restLine);
						}
					}
				}
				else
				{
					%lines = getRecordCount(%doodad);
					for(%l = 0; %l < %lines; %l++)
					{
						%line = getRecord(%doodad, %l);
						%quotPos = strPos(%line, "\"") + 2;
						%brickName = getSubStr(%line, 0, %quotPos);
						%restLine = getSubStr(%line, %quotPos, strLen(%line));
						%restLine = vectorAdd(%doodadOffset, getWords(%restLine, 0, 2)) SPC getWords(%restLine, 3, 11);
						if($AddOn__Support_AutoSaver != 1)
							$RFW::FileObject.writeLine(%brickName @ %restLine);
					}
				}
			}
		}
		//gap-finding algorithm
		%otherZx = mFloatLength($RFW::World::Z[%x + 1, %y] / $RFW::WorldBasisSizeZ, 0) * $RFW::WorldBasisSizeZ;
		%otherZy = mFloatLength($RFW::World::Z[%x, %y + 1] / $RFW::WorldBasisSizeZ, 0) * $RFW::WorldBasisSizeZ;
		while(((%z - %otherZx) > $RFW::WorldBasisSizeZActuale) || ((%z - %otherZy) > $RFW::WorldBasisSizeZActual))
		{
			%z-= $RFW::WorldBasisSizeZActual;
			if($AddOn__Support_AutoSaver != 1)
				$RFW::FileObject.writeLine($RFW::WorldBasisName @ "\" " @ setWord(%pos, 2, %z) SPC 0 SPC 1 SPC $RFW::ResourceColor[%m] @ "  " @ $RFW::ResourceColorFx[%m] SPC 0 SPC 1 SPC 1 SPC 1);
		}
		if((%otherZx - %z) > $RFW::WorldBasisSizeZActual && %x < $RFW::WorldSize - 1)
		{
			%otherPos = ((%x + 1) * $RFW::WorldBasisSizeX) SPC (%y * $RFW::WorldBasisSizeY) SPC %otherZx;
			%otherRes = $RFW::World::M[%x + 1, %y];
			if(%otherRes $= "")
			{
				if(%otherZx > $RFW::WorldMountainHeight)
				{
					%otherRes = $RFW::Resource["Stone"];
				}
				else
				{
					%otherRes = $RFW::Resource["Dirt"];
				}
			}
			while((%otherZx - %z) > $RFW::WorldBasisSizeZActual)
			{
				%otherZx-= $RFW::WorldBasisSizeZActual;
				if($AddOn__Support_AutoSaver != 1)
					$RFW::FileObject.writeLine($RFW::WorldBasisName @ "\" " @ setWord(%otherPos, 2, %otherZx) SPC 0 SPC 1 SPC $RFW::ResourceColor[%otherRes] @ "  " @ $RFW::ResourceColorFx[%otherRes] SPC 0 SPC 1 SPC 1 SPC 1);
			}
		}
		if((%otherZy - %z) > $RFW::WorldBasisSizeZActual && %y < $RFW::WorldSize - 1)
		{
			%otherPos = (%x * $RFW::WorldBasisSizeX) SPC ((%y + 1) * $RFW::WorldBasisSizeY) SPC %otherZy;
			%otherRes = $RFW::World::M[%x, %y + 1];
			if(%otherRes $= "")
			{
				if(%otherZy > $RFW::WorldMountainHeight)
				{
					%otherRes = $RFW::Resource["Stone"];
				}
				else
				{
					%otherRes = $RFW::Resource["Dirt"];
				}
			}
			while((%otherZy - %z) > $RFW::WorldBasisSizeZActual)
			{
				%otherZy-= $RFW::WorldBasisSizeZActual;
				if($AddOn__Support_AutoSaver != 1)
					$RFW::FileObject.writeLine($RFW::WorldBasisName @ "\" " @ setWord(%otherPos, 2, %otherZy) SPC 0 SPC 1 SPC $RFW::ResourceColor[%otherRes] @ "  " @ $RFW::ResourceColorFx[%otherRes] SPC 0 SPC 1 SPC 1 SPC 1);
			}
		}
		//iterate
		if(%y == $RFW::WorldSize - 1)
		{
			%x++;
			if(%x == $RFW::WorldSize - 1)
			{
				echo("RFW: Saving complete.");
				if($AddOn__Support_AutoSaver != 1)
				{
					$RFW::FileObject.close();
					$RFW::FileObject.delete();
				}
				else
					Autosaver_Begin("ReverieFortWars");

				deleteVariables("RFW::World::*");
				schedule(1, 0, ServerDirectSaveFileLoad, "saves/RFW.bls", 3, 0, 2);
				return;
			}
			%y = 0;
		}
		else
		{
			%y++;
		}
	}
	$WorldBuildSched = schedule(1, 0, ReverieFortWars_ConstructWorld, %x, %y);
}

// #2.6
function ReverieFortWars_HydraulicErosion(%i)
{
	if($RFW::Erosion::Cycles == 0)
	{
		return ReverieFortWars_ConstructWorld(0, 0);
	}
	if(isEventPending($WorldPlanSched))
	{
		return schedule(100, 0, ReverieFortWars_HydraulicErosion, %i);
	}
	echo("Hydraulic erosion cycle " @ %i);
	schedule(1, 0, RFW_HE_Rain);
	RFW_HE_Erode();
	RFW_HE_Settle();
	RFW_HE_Evaporate();
	if(%i++ < $RFW::Erosion::Cycles)
	{
		return schedule(10, 0, ReverieFortWars_HydraulicErosion, %i);
	}
	else
	{
		return ReverieFortWars_ConstructWorld(0, 0);
	}
}

// #3.

function RFW_AddBiome(%centerX, %centerY, %radius, %type)
{
	%xMin = mClamp(%centerX - %radius, 0, $RFW::WorldSize);
	%xMax = mClamp(%centerX + %radius, 0, $RFW::WorldSize);
	%yMin = mClamp(%centerY - %radius, 0, $RFW::WorldSize);
	%yMax = mClamp(%centerY + %radius, 0, $RFW::WorldSize);
	for(%x = %xMin; %x <= %xMax; %x++)
	{
		for(%y = %yMin; %y <= %yMax; %y++)
		{
			//%relDist = mSqrt(mPow(%centerX - %x, 2) + mPow(%centerY - %y, 2)) / %radius;
			%relDist = vectorDist(%x SPC %y, %centerX SPC %centerY) / %radius;
			if(%relDist < 1)
			{
				if(%relDist > 0.7)
				{
					%relDist = (1 - %relDist) / 0.3;
					if(getRandom() < %relDist)
					{
						$RFW::World::M[%x, %y] = %type;
					}
				}
				else
				{
					$RFW::World::M[%x, %y] = %type;
				}
			}
		}
	}
}

function addDoodad(%brick, %doodad)
{
	%pos = %brick.getPosition();
	for(%i = 0 ; %i < getLineCount(%doodad); %i++)
	{
		%data = getLine(%doodad, %i);
		%nbrick = new fxDtsBrick()
		{
			datablock = nameToID(getField(%data, 0));
			position = vectorAdd(%pos, getField(%data, 1));
			rotation = rotationFromAngleID(getField(%data, 2)); //stupid voodoo magic
			colorID = getField(%data, 3);
			colorFxID = getField(%data, 4);
			//material = getField(%data, 5);
			planted = true;
		};
		%err = %nbrick.plant();
		if(!%err)
		{
			%brick.getGroup().add(%nbrick);
			%nbrick.plantedTrustCheck();
		}
		else
		{
			%nbrick.delete();
		}
	}
}

function RFW_HE_Rain()
{
	for(%x = 0; %x < $RFW::WorldSize; %x++)
	{
		for(%y = 0; %y < $RFW::WorldSize; %y++)
		{
			//add rain
			$RFW::World::Water[%x, %y]+= $RFW::Erosion::RainAmt;
		}
	}
	RFW_HE_Erode();
}

function RFW_HE_Erode()
{
	for(%x = 0; %x < $RFW::WorldSize; %x++)
	{
		for(%y = 0; %y < $RFW::WorldSize; %y++)
		{
			//erode the land
			$RFW::World::Z[%x, %y]-= $RFW::Erosion::Solubility;
			$RFW::World::Sediment[%x, %y]+= $RFW::Erosion::Solubility;
		}
	}
	RFW_HE_Settle();
}

function RFW_HE_Settle()
{
	for(%x = 0; %x < $RFW::WorldSize; %x++)
	{
		for(%y = 0; %y < $RFW::WorldSize; %y++)
		{
			//find local land heights
			%zC = $RFW::World::Z[%x, %y];
			%zU = $RFW::World::Z[%x, %y + 1];
			%zD = $RFW::World::Z[%x, %y - 1];
			%zL = $RFW::World::Z[%x - 1, %y];
			%zR = $RFW::World::Z[%x + 1, %y];
			//find local water amounts
			%wC = $RFW::World::Water[%x, %y];
			%wU = $RFW::World::Water[%x, %y + 1];
			%wD = $RFW::World::Water[%x, %y - 1];
			%wL = $RFW::World::Water[%x - 1, %y];
			%wR = $RFW::World::Water[%x + 1, %y];
			//find how many of these aren't blank
			%denom = 1 + (%zU != 0) + (%zD != 0) + (%zL != 0) + (%zR != 0);
			%wAvg = (%wC + %wU + %wD + %wL + %wR) / %denom;
			%zAvg = (%zC + %zU + %zD + %zL + %zR) / %denom;
			%hAvg = %wAvg + %zAvg;
			//we need to exclude tiles whose land height is greater than the average water+land height
			if(%zC > %hAvg)
			{
				%wAvg = %wAvg * %denom / (%denom + 1);
				%zAvg = %zAvg * %denom / (%denom + 1);
				$RFW::World::Water[%x, %y] = 0;
				%denom--;
			}
			if(%zU > %hAvg)
			{
				%wAvg = %wAvg * %denom / (%denom + 1);
				%zAvg = %zAvg * %denom / (%denom + 1);
				$RFW::World::Water[%x, %y + 1] = 0;
				%denom--;
			}
			if(%zD > %hAvg)
			{
				%wAvg = %wAvg * %denom / (%denom + 1);
				%zAvg = %zAvg * %denom / (%denom + 1);
				$RFW::World::Water[%x, %y - 1] = 0;
				%denom--;
			}
			if(%zL > %hAvg)
			{
				%wAvg = %wAvg * %denom / (%denom + 1);
				%zAvg = %zAvg * %denom / (%denom + 1);
				$RFW::World::Water[%x - 1, %y] = 0;
				%denom--;
			}
			if(%zR > %hAvg)
			{
				%wAvg = %wAvg * %denom / (%denom + 1);
				%zAvg = %zAvg * %denom / (%denom + 1);
				$RFW::World::Water[%x + 1, %y] = 0;
				%denom--;
			}
			//redistribute water
			%waterLevel = %zAvg + %wAvg;
			if(%zC <= %hAvg)
			{
				$RFW::World::Water[%x, %y] = %waterLevel - %zC;
			}
			if(%zU && %zU <= %hAvg)
			{
				$RFW::World::Water[%x, %y + 1] = %waterLevel - %zU;
			}
			if(%zD && %zD <= %hAvg)
			{
				$RFW::World::Water[%x, %y - 1] = %waterLevel - %zD;
			}
			if(%zL && %zL <= %hAvg)
			{
				$RFW::World::Water[%x - 1, %y] = %waterLevel - %zL;
			}
			if(%zR && %zR <= %hAvg)
			{
				$RFW::World::Water[%x + 1, %y] = %waterLevel - %zR;
			}
			//redistribute sediment
			%sAvg = ($RFW::World::Sediment[%x, %y] + $RFW::World::Sediment[%x, %y + 1] + $RFW::World::Sediment[%x, %y - 1] + $RFW::World::Sediment[%x - 1, %y] + $RFW::World::Sediment[%x + 1, %y]) / %denom;
			%sPerW = %sAvg / %wAvg;
			$RFW::World::Sediment[%x, %y] = %sPerW * $RFW::World::Water[%x, %y];
			$RFW::World::Sediment[%x, %y + 1] = %sPerW * $RFW::World::Water[%x, %y + 1];
			$RFW::World::Sediment[%x, %y - 1] = %sPerW * $RFW::World::Water[%x, %y - 1];
			$RFW::World::Sediment[%x - 1, %y] = %sPerW * $RFW::World::Water[%x - 1, %y];
			$RFW::World::Sediment[%x + 1, %y] = %sPerW * $RFW::World::Water[%x + 1, %y];
		}
	}
	RFW_HE_Evaporate();
}

function RFW_HE_Evaporate()
{
	for(%x = 0; %x < $RFW::WorldSize; %x++)
	{
		for(%y = 0; %y < $RFW::WorldSize; %y++)
		{
			//evaporate water
			$RFW::World::Water[%x, %y] = mClampF($RFW::Water[%x, %y] - $RFW::Erosion::Evaporation, 0, 1000);
			//deposit excess sediment, if any
			%maxSed = $RFW::World::Water[%x, %y] * $RFW::Erosion::Capacity;
			if($RFW::World::Sediment[%x, %y] > %maxSed)
			{
				%diff = %maxSed - $RFW::World::Sediment[%x, %y];
				$RFW::World::Z[%x, %y]+= %diff;
				$RFW::World::Sediment[%x, %y]-= %diff;
			}
		}
	}
}