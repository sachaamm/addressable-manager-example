
# unity-addressable-manager-example
Example of simple addressable manager handling remote unavailable and resource location fallback

## Testing the concept

1. Load SampleScene
2. Build Addressable Content
3. Build executable
4. Modify an Addressable Asset
5. Update Previous Build
6. Start Editor Hosting -> Window -> AssetManagment -> Addressable -> Hosting : toggle Enable
7. Launch the executable -> Thanks to Remote Catalog Changes are propagated

If you stop the Editor Host, the Executable will fallback to ServerData

## Importing in your project
(Import Addressable in your project and create addressable settings)
1. Import AddressableManager.cs and AddressableManagerRemoteChecker.cs in your project
2. Create an Addressable Group, and copy all configuration as it is in this project for the DefaultGroup
3. Create a script that extends AddressableManager ( See MyAddressableManager for convenience )
4. Create a scriptable object for RemoteChecker ( SO/AddressableManager/RemoteChecker )
5. Set this scriptable object as an Addressable in the Group created Step 2. with the key "REMOTE_CHECKER"
6. Follow Same Procedure for Testing the concept below section in order to integrate in the project
