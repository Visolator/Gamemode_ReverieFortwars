//By Chrono

package InventorySlotAdjustment
{
	function Armor::onNewDatablock(%data,%this)
	{
		%p = Parent::onNewDatablock(%data,%this);
		if(isObject(%this.client))
		{
			commandToClient(%this.client,'PlayGui_CreateToolHud',%data.maxTools);
			for(%i = 0; %i < %data.maxTools; %i++)
			{
				if(isObject(%this.tool[%i]))
				{
					messageClient(%this.client,'MsgItemPickup',"",%i,%this.tool[%i].getID(),1);
				}
				else
				{
					messageClient(%this.client,'MsgItemPickup',"",%i,0,1);
				}
			}
		}
		return %p;
	}
	function GameConnection::setControlObject(%this,%obj)
	{
		%p = Parent::setControlObject(%this,%obj);
		if(%obj == %this.player)
		{
			commandToClient(%this,'PlayGui_CreateToolHud',%obj.getDatablock().maxTools);
		}
		return %p;
	}
	function Player::changeDatablock(%this,%data,%client)
	{
		if(%data != %this.getDatablock())
		{
			commandToClient(%this.client,'PlayGui_CreateToolHud',%data.maxTools);
		}
		%p = Parent::changeDatablock(%this,%data,%client);
		return %p;
	}
};
activatePackage(InventorySlotAdjustment);