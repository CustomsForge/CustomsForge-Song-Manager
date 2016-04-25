The project PreBuild script will automatically update 
the project ThirdParty and lib support files as needed.

These three xml files must be present in the application folder.
Otherwise CFSM (RocksmithToolkitLib.dll) will throw a not initialized error.

RocksmithToolkitLib.Config.xml
RocksmithToolkitLib.SongAppId.xml
RocksmithToolkitLib.TuningDefinition.xml

ww2ogg executables are required by RocksmithToolkitLib for proper operation for now

The following are not .Net native libraries.
These may not be added as References in the application (will cause error)
They need to be installed in the root application path to work correctly
Set Build Action as Content and Copy to Output to Copy always
These three dll files must be present in the application folder

7za.dll
bass.dll
DF_DDSImage.dll