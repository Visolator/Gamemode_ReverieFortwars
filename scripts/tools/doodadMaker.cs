//data" x y z angleid ?(1) color print(leave blank for non print bricks) colorfx? shapefx? raycasting? colliding? rendering?

function getFriendlyDoodad(%brick, %pos)
{
	%doodad = getDoodad(%brick, %pos);
	%doodad = strReplace(%doodad, "\n", "\\n");
	%doodad = strReplace(%doodad, "\"", "\\\"");
}

function multiDoodad(%brick, %biome, %num)
{
	for(%i = 0; %i < $Doodad[%biome, %num]; %i++)
	{
		setDoodad(%brick, $Doodad[%biome, %num, %i]);
	}
}

function setDoodad(%brick, %doodad)
{
	%basePos = %brick.getPosition();
	%count = getRecordCount(%doodad);
	for(%i = 0; %i < %count; %i++)
	{
		%record = getRecord(%doodad, %i);
		%dataEnd = strPos(%record, "\"");
		%data = $uiNameTable[getSubStr(%record, 0, %dataEnd)];
		%record = getSubStr(%record, %dataEnd + 2, strLen(%record));
		%pos = vectorAdd(%basePos, getWords(%record, 0, 2));
		%angleID = getWord(%record, 3);
		%colorID = getWord(%record, 5);
		if(%data.hasPrint)
		{
			%print = getWord(%record, 6);
			%shapeFX = getWord(%record, 7);
			%colorFX = getWord(%record, 8);
			%raycasting = getWord(%record, 9);
			%colliding = getWord(%record, 10);
			%rendering = getWord(%record, 11);
		}
		else
		{
			%shapeFX = getWord(%record, 6);
			%colorFX = getWord(%record, 7);
			%raycasting = getWord(%record, 8);
			%colliding = getWord(%record, 9);
			%rendering = getWord(%record, 10);
		}
		%newBrick = new fxDtsBrick()
		{
			datablock = %data;
			position = %pos;
			rotation = rotationFromAngleID(%angleID);
			angleID = %angleID;
			colorID = %colorID;
			colorFxID = %colorFX;
			shapeFxID = %shapeFX;
			raycasting = %raycasting;
			colliding = %colliding;
			rendering = %rendering;
			isPlanted = true;
			client = %brick.client;
		};
		%brick.getGroup().add(%newBrick);
		if((%err = %newBrick.plant()) != 0)
		{
			%newBrick.delete();
		}
		else
		{
			%newBrick.plantedTrustCheck();
		}
	}
}

function getDoodad(%brick, %pos)
{
	setDoodadTimeout(%brick);
	if(%pos $= "")
	{
		%pos = %brick.getPosition();
	}
	%count = %brick.getNumUpBricks();
	for(%i = 0; %i < %count; %i++)
	{
		%obrick = %brick.getUpBrick(%i);
		if(!%obrick.doodaded)
		{
			setDoodadTimeout(%obrick);
			%str = %str NL %obrick.getDatablock().uiName @ "\" " @ vectorSub(%obrick.getPosition(), %pos) SPC %obrick.getAngleID() SPC "1" SPC %obrick.getColorID() SPC %obrick.getPrintID() SPC %obrick.getColorFxID() SPC "0 1 1 1";
			if(%obrick.getNumUpBricks() || %obrick.getNumDownBricks())
			{
				%str = %str NL getDoodad(%obrick, %pos);
			}
		}
	}
	%count = %brick.getNumDownBricks();
	for(%i = 0; %i < %count; %i++)
	{
		%obrick = %brick.getDownBrick(%i);
		if(!%obrick.doodaded)
		{
			setDoodadTimeout(%obrick);
			%str = %str NL %obrick.getDatablock().uiName @ "\" " @ vectorSub(%obrick.getPosition(), %pos) SPC %obrick.getAngleID() SPC "1" SPC %obrick.getColorID() SPC %obrick.getPrintID() SPC %obrick.getColorFxID() SPC "0 1 1 1";
			if(%obrick.getNumUpBricks() || %obrick.getNumDownBricks())
			{
				%str = %str NL getDoodad(%obrick, %pos);
			}
		}
	}
	return trim(%str);
}

function setDoodadTimeout(%brick)
{
	%brick.doodaded = true;
	schedule(1, 0, eval, %brick @ ".doodaded = false;");
	%brick.schedule(1, setColor, 2);
	%brick.schedule(1, setColorFx, 6);
	%brick.schedule(2500, setColor, %brick.getColorID());
	%brick.schedule(2500, setColorFx, %brick.getColorFxID());
}