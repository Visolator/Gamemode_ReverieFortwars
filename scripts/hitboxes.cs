//--------------------
// Name: Hitboxes
// Type: Hack
// Version: 1.0
// Author: Jookia (ID 119)
//--------------------

if(isPackage(hitboxes) == true)
	return; // Don't need duplicates.

package hitboxes
{
	function getHitbox(%obj, %col, %pos)
	{
		$hitbox = true;
		$hitboxList = "";
		
		paintProjectile::onCollision("", %obj, %col, "", %pos, "");
		cancel(%col.tempColorSchedule);
		$hitbox = "";
		
		%hitboxList = $hitboxList;
		$hitboxList = "";
		
		return %hitboxList;
	}
	function shapeBase::setNodeColor(%obj, %node, %color)
	{
		if($hitbox == true)
		{
			if($hitboxList $= "")
				$hitboxList = %node;
			else
				$hitboxList = $hitboxList SPC %node;
			
			return;
		}
		parent::setNodeColor(%obj, %node, %color);
	}
};
activatePackage(hitboxes);