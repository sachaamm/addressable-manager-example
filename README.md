# unity-addressable-manager-example
Example of simple addressable manager handling remote unavailable and resource location fallback

1. Load SampleScene
2. Build Addressable Content
3. Build executable
4. Modify an Addressable Asset
5. Update Previous Build
6. Start Editor Hosting -> Window -> AssetManagment -> Addressable -> Hosting : toggle Enable
7. Launch the executable -> Thanks to Remote Catalog Changes are propagated

If you stop the Editor Host, the Executable will fallback to ServerData

