//ReverieFortWars/resources.cs

//TABLE OF CONTENTS
// #1. documentation
// #2. resource enumeration
// #3. resource functionality

// #1.
//$RFW::Resources             | Total number of resources.
//$RFW::Resource[%int]        | The name of resource %int (%int is in the range 0 .. $RFW::Resources-1)
//$RFW::Resource[%str]        | The resource number of the resource named %str
//$RFW::ResourceQuality[%int] | The quality of resource %int, an int in the range 0 .. 100. Determines the durability per unit volume of placed bricks of this resource type.
//$RFW::ResourceTier[%int]    | The lowest tier of weapons that can destroy/harvest this material (1=wood, 2=stone, 3=surtra/obsidian, 4=calcite/nithlite, 5=sanguite/sinnite, 6=crystal)
//$RFW::ResourceColor[%int]   | The default paint color to use to color bricks of this resource.
//$RFW::ResourceColorFX[%int] | The default color effect to use for bricks of this resource. (0=none, 1=pearl, 2=chrome, 3=glow, 4=blink, 5=swirl, 6=rainbow)

// #2.
$RFW::Resources = 0;

function RFW_RegisterResource(%name, %quality, %tier, %colors, %colorFx)
{
	%x = $RFW::Resources++;
	$RFW::Resource[%x] = %name;
	$RFW::Resource[%name] = %x;
	$RFW::ResourceQuality[%x] = %quality;
	$RFW::ResourceQuality[%name] = %quality;
	$RFW::ResourceTier[%x] = %tier;
	$RFW::ResourceTier[%name] = %tier;
	%color = getWord(%colors, 0);
	$RFW::ResourceColor[%x] = %color;
	$RFW::ResourceColor[%name] = %color;
	$RFW::ResourceByColor[%color] = %name;
	for(%i = 1; %i < getWordCount(%colors); %i++)
	{
		%color = getWord(%colors, %i);
		$RFW::ResourceByColor[%color] = %name;
	}
	$RFW::ResourceColorFx[%x] = %colorFx;
	$RFW::ResourceColorFx[%name] = %colorFx;
}


RFW_RegisterResource("Dirt", 5, 0, "0", 0);
RFW_RegisterResource("Plant matter", 5, 0, "3", 0);
RFW_RegisterResource("Sand", 5, 0, "1", 0);
RFW_RegisterResource("Glass", 5, 0, "28 29 30 31 32 33 34 35 36", 1);
RFW_RegisterResource("Cloth", 10, 0, "19 20 21 22 23 24 25 26 27", 0);
RFW_RegisterResource("Snow", 10, 0, "2", 0);
RFW_RegisterResource("Flesh", 10, 0, "17", 1);
RFW_RegisterResource("Bone", 20, 0, "18", 0);
RFW_RegisterResource("Wood", 25, 0, "4", 0);
RFW_RegisterResource("Stone", 35, 1, "6 7 8 9 10", 0);
RFW_RegisterResource("Obsidian", 50, 2, "5", 1);
RFW_RegisterResource("Surtra", 50, 2, "11", 1);
RFW_RegisterResource("Calcite", 75, 3, "12", 1);
RFW_RegisterResource("Nithlite", 75, 3, "13", 2);
RFW_RegisterResource("Sanguite", 100, 4, "14", 1);
RFW_RegisterResource("Sinnite", 100, 4, "15", 2);
RFW_RegisterResource("Crystal", 125, 5, "16", 1);

// #3.
function getBiome(%pos)
{
	%pos = vectorAdd(%pos, "0 0 10");
	%end = vectorAdd(%pos, "0 0 -100");
	%ray = containerRaycast(%pos, %end, $Typemasks::FxBrickObjectType);
	while(isObject(%brick = firstWord(%ray)))
	{
		%data = %brick.getDatablock();
		if(%brick.getGroup().bl_id == 888888 && (%data.isCube || %data == nameToID(brick16x16fData)))
		{
			%res = $RFW::ResourceByColor[%brick.colorID];
			if(%res $= "Sand" || %res $= "Snow" || %res $= "Stone" || %res $= "Flesh")
			{
				return %res;
			}
			else
			{
				return "Grass";
			}
			break;
		}
		else
		{
			if(%crashCheck++ > 25)
			{
				error("getBiome crashcheck failed for pos " @ %pos);
				return -1;
			}
			%ray = containerRaycast(posFromRaycast(%ray), %end, $Typemasks::FxBrickObjectType, %brick);
		}
	}
	return 0;
}