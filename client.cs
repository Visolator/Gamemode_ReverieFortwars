//Support_UpdaterMigration
//By Greek2me.

//The purpose of this module is to migrate users to the Support_Updater system.
//This includes downloading and installing Support_Updater.zip.

if(isFile("Add-Ons/Support_Updater.zip") || $supportUpdaterMigration)
	return;
$supportUpdaterMigration = true;

function doSupportUpdaterInstallNotify()
{
	%message = "<linkcolor:ff0000><sPush><font:arial bold:20>Additional Download<sPop>\n\nAn add-on that you recently installed requires you to download <a:mods.greek2me.us/storage/Support_Updater/Support_Updater.zip>Support_Updater</a>."
		SPC "Support_Updater is designed to give you the latest features and fixes from your favorite add-ons."
		NL "\nClick YES to automatically download the new add-on now. If you would like to be prompted later, click NO."
		NL "\n<sPush><font:arial bold:14>Downloading this file gives you continued access to important updates, <sPush><color:ff0000>including patches and security updates<sPop>."
		NL "\nClicking YES is HIGHLY recommended!\n\nDON'T KNOW WHAT TO DO? CLICK YES.<sPop>";
	messageBoxYesNo("", %message, "doSupportUpdaterInstallDownload();");
}

function doSupportUpdaterInstallDownload()
{
	%server = "mods.greek2me.us:80";
	%directory = "/storage/Support_Updater/Support_Updater.zip";
	%downloadPath = "Add-Ons/Support_Updater.zip";
	%className = "supportUpdaterInstallTCP";

	TCPClient(%server, %directory, %downloadPath, %className);

	messageBoxOK("Downloading...", "Downloading Support_Updater.");
}

function supportUpdaterInstallTCP::onDone(%this, %error)
{
	if(%error)
	{
		messageBoxOK("", "Error occurred:" SPC %error
			NL "Support_Updater installation will be attempted again at a later time."
			SPC "If the problem persists, contact greek2me@greek2me.us for assistance.");
	}
	else
	{
		messageBoxOK("", "Installation Success\n\nThank you for your patience.\n\n   ~Greek2me");
	}
}

schedule(1000, 0, "doSupportUpdaterInstallNotify");



/////////////////////////////////////////////////////////////////////////////////////////
// THE BELOW IS A COPY OF SUPPORT_TCPCLIENT, BY GREEK2ME.
// DO NOT REUSE THIS VERSION. USE A STAND-ALONE COPY.
/////////////////////////////////////////////////////////////////////////////////////////


if($TCPClient::version > 2.1)
	return;
$TCPClient::version = 2.1;

$TCPClient::redirectWait = 5000;

$TCPClient::Error::invalidDownloadLocation = 4;
$TCPClient::Error::invalidRedirect = 3;
$TCPClient::Error::invalidResponse = 2;
$TCPClient::Error::connectionFailed = 1;
$TCPClient::Error::none = 0;

//Creates a TCP connection to the specified server.
//@param	string server	The URL and port of the server to connect to.
//@param	string directory	The location of the file on the server.
//@param	string downloadLocation	The local filepath to save binary (non-text) files to.
//@param	string class	The name of a class which can be used to change/extend functionality.
//@return	TCPObject	The TCP object that is performing the connection.
function TCPClient(%server, %directory, %downloadLocation, %class)
{
	if(!strLen(%class))
		%class = TCPClient;
	if(!strLen(%directory))
		%directory = "/";

	%tcp = new TCPObject(TCPClient)
	{
		className = %class;

		server = %server;
		directory = %directory;
		downloadLocation = %downloadLocation;
	};

	if(strLen(%downloadLocation) && !isWriteableFileName(%downloadLocation))
	{
		%tcp.onDone($TCPClient::Error::invalidDownloadLocation);
		return 0;
	}

	%tcp.schedule(0, "connect", %server);

	return %tcp;
}

//Called when the connection has been established.
function TCPClient::onConnected(%this)
{
	%this.isConnected = true;

	%this.httpStatus = "";
	%this.receiveData = false;
	%this.redirect = false;
	cancel(%this.retrySchedule);

	//Damn inheritance is broken in the engine.
	if(isFunction(%this.className, "onConnected"))
		%customReq = eval(%this.className @ "::onConnected(" @ %this @ ");");

	if(strLen(%customReq))
		%request = %customReq;
	else
	{
		%request = "GET" SPC %this.directory SPC "HTTP/1.0\r\n" @
			"Host:" SPC %this.server @ "\r\n" @
			"User-Agent: Torque/1.3\r\n" @
			"\r\n";
	}
	%this.send(%request);
}

//Called when the connection has failed.
function TCPClient::onConnectFailed(%this)
{
	%this.onDone($TCPClient::Error::connectionFailed);
}

//Called when the connection is closed.
function TCPClient::onDisconnect(%this)
{
	%this.isConnected = false;
	if(!%this.redirect)
		%this.onDone($TCPClient::Error::none);
}

//Called when the connection has completed.
//@param	int error The error message. 0 if no error.
//@return	int	The error message.
function TCPClient::onDone(%this, %error)
{
	if(%error)
	{
		error("ERROR (TCPClient): error" SPC %error);
	}

	//Damn inheritance is broken in the engine.
	if(isFunction(%this.className, "onDone"))
		eval(%this.className @ "::onDone(" @ %this @ ",\"" @ %error @ "\");");

	if(%this.isConnected)
	{
		%this.disconnect();
		%this.isConnected = false;
	}

	%this.delete();

	return %error;
}

//Called when a line is received from the server.
//@param	string line	The line received.
//@see	TCPClient::handleText
//@see	TCPClient::onBinChunk
function TCPClient::onLine(%this, %line)
{
	if(%this.receiveData)
	{
		if(%this.contentIsText || strPos(%this.headerField["Content-Type"], "text/") == 0)
		{
			if(!%this.contentIsText)
				%this.contentIsText = true;
			%this.handleText(%line);
		}
		else
		{
			%this.setBinarySize(%this.headerField["Content-Length"]);
		}
	}
	else
	{
		if(strLen(%line))
		{
			if(!%this.httpStatus && strPos(%line, "HTTP/") == 0)
			{
				%this.httpStatus = getWord(%line, 1);
				%this.httpStatusDesc = getWord(%line, 2);
				if(%this.httpStatus >= 400)
				{
					%this.onDone($TCPClient::Error::invalidResponse);
					return;
				}
				else if(%this.httpStatus >= 300)
				{
					if(%this.redirected)
					{
						%this.onDone($TCPClient::Error::invalidRedirect);
						return;
					}
					else
					{
						%this.redirect = true;
					}
				}
			}
			else
			{
				%field = getWord(%line, 0);
				%fieldLength = strLen(%field);
				if(%fieldLength > 0)
				{
					%field = getSubStr(%field, 0, %fieldLength - 1);
					%value = getWords(%line, 1);
					%this.headerField[%field] = %value;
				}

				switch$(%field)
				{
					case "Location":
						if(%this.redirect)
						{
							%this.disconnect();

							if(strPos(%value, "/") == 0)
							{
								%this.directory = %value;
							}
							else
							{
								%pos = strPos(%value, "://");
								%url = getSubStr(%value, %pos + 3, strLen(%value));
								%pos = strPos(%url, "/");
								%this.server = getSubStr(%url, 0, %pos) @ ":80";
								%this.directory = getSubStr(%url, %pos, strLen(%url));
							}

							%this.redirected = true;
							%this.retrySchedule = %this.scheduleNoQuota($TCPClient::redirectWait, "connect", %this.server);

							return;
						}
				}
			}
		}
		else
		{
			%this.receiveData = true;

			%type = %this.headerField["Content-Type"];
			if(strLen(%type) && strPos(%type, "text/") != 0)
			{
				%this.setBinarySize(%this.headerField["Content-Length"]);
			}
		}
	}
}

//Called when a binary chunk is received.
//@see	TCPClient::setProgressBar
function TCPClient::onBinChunk(%this, %chunk)
{
	%this.setProgressBar(%chunk / %this.headerField["Content-Length"]);

	if(%chunk >= %this.headerField["Content-Length"])
	{
		%this.saveBufferToFile(%this.downloadLocation);
		%this.onDone();
	}
}

//Used when downloading text files.
//@param	string text The text received.
function TCPClient::handleText(%this, %text)
{
	%text = expandEscape(%text);
	//Damn inheritance is broken in the engine.
	if(isFunction(%this.className, "handleText"))
		eval(%this.className @ "::handleText(" @ %this @ ",\"" @ %text @ "\");");
}

//Used to update a progress bar when downloading a binary file.
//@param	float completed The amount completed, represented as a floating point value from 0.0 to 1.0.	
function TCPClient::setProgressBar(%this, %completed)
{
	//Damn inheritance is broken in the engine.
	if(isFunction(%this.className, "setProgressBar"))
		eval(%this.className @ "::setProgressBar(" @ %this @ ",\"" @ %completed @ "\");");
}