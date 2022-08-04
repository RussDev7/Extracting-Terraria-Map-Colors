# Extracting Colors From The Terraria Map


 **Step1: Generating The Map**
 
For this, I'm using an open source application called [TEdit](https://github.com/TEdit/Terraria-Map-Editor) and replacing a function called `LeadBestiary()`. This function will go through and place all 624 tiles and 315 walls for each possible paint color. Leaving one unpainted. Due to the logic of Terraria, some blocks such as sand will need to be resting on a solid block. For this we place a glass under each block, ignoring walls.

![WorldPeak](https://user-images.githubusercontent.com/33048298/182737324-46cccf11-4f6b-46ed-b521-0f0144494b2b.PNG)

<details><summary>Show Code</summary>
 
```c#
public void LoadBestiary()
{
    // Stage World Vars
    int minx = 100;
    int maxx = this._wvm.CurrentWorld.TilesWide - 100;
    int miny = 100;
    int maxy = this._wvm.CurrentWorld.TilesHigh - 100;
    
    // Reset Vars
    int tile = 0;
    int paint = 0;
    bool useGlass = false;
    
    // First Do Tiles
    for (int x = minx; x < maxx; x++)
    {
        for (int y = miny; y < maxy; y++)
        {
            try
            {
                if (!useGlass)
                {
                    this._wvm.CurrentWorld.Tiles[x, y].Type = (ushort)tile;
                    this._wvm.CurrentWorld.Tiles[x, y].IsActive = true;
                    this._wvm.CurrentWorld.Tiles[x, y].TileColor = (byte)paint;
                    if (tile == 624 && paint == 31)
                    {
                        // Define New Vars
                        minx = (x + 2);
                        goto LeaveTileLoop;
                    }

                    if (paint == 31)
                    {
                        tile++;
                        paint = 0;
                    }
                    else
                    {
                        paint++;
                    }

                    useGlass = true;
                }
                else
                {
                    this._wvm.CurrentWorld.Tiles[x, y].Type = (ushort)54;
                    this._wvm.CurrentWorld.Tiles[x, y].IsActive = true;
                    useGlass = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error.");
            }
        }

        // Offset Right
        x++;
    }

    LeaveTileLoop:
    
    // Reset Vars
    tile = 1;
    paint = 0;
    
    // Next Do Walls
    for (int x = minx; x < maxx; x++)
    {
        for (int y = miny; y < maxy; y++)
        {
            try
            {
                this._wvm.CurrentWorld.Tiles[x, y].Wall = (ushort)tile;
                this._wvm.CurrentWorld.Tiles[x, y].WallColor = (byte)paint;
                if (tile == 315 && paint == 31)
                {
                    // Define New Vars
                    minx = x;
                    goto LeaveWallLoop;
                }

                if (paint == 31)
                {
                    tile++;
                    paint = 0;
                }
                else
                {
                    paint++;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error.");
            }
        }
    }

    LeaveWallLoop:
        System.Windows.Forms.MessageBox.Show("Finished.");
}
```
</details>

 **Step2: Reading The Map**
 
 Now that we have created a map with every possible tile and wall, its time we attempt to try and read the colors just as the game would. To do this, I will be using an open source tool called [DnSpy](https://github.com/dnSpy/dnSpy) to edit the games compiled code. Using one of the games functions called `OpenInventory()` I'm able to add some code onto this that will attempt to read each tile and wall starting from x:100, y:100, through to the end of the map file.
 
<details><summary>Show Code</summary>
 
 ```c#
private static void OpenInventory()
{
    int minTilesX = 100;
    int maxTilesX = Main.maxTilesX;
    int minTilesY = 100;
    int maxTilesY = Main.maxTilesY;
    for (int i = minTilesX; i < maxTilesX; i++)
    {
        for (int j = minTilesY; j < maxTilesY; j++)
        {
            try
            {
                MapTile mapTile = Main.Map[i, j];
                if (Main.tile[i, j].wall != 0)
                {
                    File.AppendAllText("C:\\Program Files (x86)\\Steam\\steamapps\\common\\Terraria\\colors.txt", string.Concat(new object[]{MapHelper.GetMapTileXnaColor(ref mapTile).Hex3().ToUpper(), "|", "WALL: " + Main.tile[i, j].wall, "|", GetPaintFromByte(Main.tile[i, j].wallColor()), Environment.NewLine}));
                }
                else
                {
                    File.AppendAllText("C:\\Program Files (x86)\\Steam\\steamapps\\common\\Terraria\\colors.txt", string.Concat(new object[]{MapHelper.GetMapTileXnaColor(ref mapTile).Hex3().ToUpper(), "|", "TILE: " + Main.tile[i, j].type, "|", GetPaintFromByte(Main.tile[i, j].color()), Environment.NewLine}));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error.");
            }
        }
    }
}

public static string GetPaintFromByte(byte color)
{
    string result = "None";
    if (color == 0)
    {
        result = "None";
    }
    else if (color == 1)
    {
        result = "Red";
    }
    else if (color == 2)
    {
        result = "Orange";
    }
    else if (color == 3)
    {
        result = "Yellow";
    }
    else if (color == 4)
    {
        result = "Lime";
    }
    else if (color == 5)
    {
        result = "Green";
    }
    else if (color == 6)
    {
        result = "Teal";
    }
    else if (color == 7)
    {
        result = "Cyan";
    }
    else if (color == 8)
    {
        result = "SkyBlue";
    }
    else if (color == 9)
    {
        result = "Blue";
    }
    else if (color == 10)
    {
        result = "Purple";
    }
    else if (color == 11)
    {
        result = "Violet";
    }
    else if (color == 12)
    {
        result = "Pink";
    }
    else if (color == 13)
    {
        result = "DeepRed";
    }
    else if (color == 14)
    {
        result = "DeepOrange";
    }
    else if (color == 15)
    {
        result = "DeepYellow";
    }
    else if (color == 16)
    {
        result = "DeepLime";
    }
    else if (color == 17)
    {
        result = "DeepGreen";
    }
    else if (color == 18)
    {
        result = "DeepTeal";
    }
    else if (color == 19)
    {
        result = "DeepCyan";
    }
    else if (color == 20)
    {
        result = "DeepSkyBlue";
    }
    else if (color == 21)
    {
        result = "DeepBlue";
    }
    else if (color == 22)
    {
        result = "DeepPurple";
    }
    else if (color == 23)
    {
        result = "DeepViolet";
    }
    else if (color == 24)
    {
        result = "DeepPink";
    }
    else if (color == 25)
    {
        result = "Black";
    }
    else if (color == 26)
    {
        result = "White";
    }
    else if (color == 27)
    {
        result = "Gray";
    }
    else if (color == 28)
    {
        result = "Brown";
    }
    else if (color == 29)
    {
        result = "Shadow";
    }
    else if (color == 30)
    {
        result = "Negative";
    }
    else if (color == 31)
    {
        result = "Illuminant";
    }

    return result;
}
  ```
</details>
    
 **Step3: Importing Map Data**
 
So now, we have a text file with every possible tile and wall color. Now we need to import it into [Microsoft Excel](https://www.microsoft.com/en-us/microsoft-365/excel) where we can further make some changes. Lets start by opening this text file using notepad.exe. Once opened, we want to replace (ctrl+h) all vertical bars `"|"` with an [horizontal tabulation](http://www.unicode-symbol.com/u/0009.html) `"	"`.  This will allow us to then import this into Excel via select all, copy, and then paste.
 
 
 **Step4: Formatting The Map Data**
 
 So now, we have a text file imported into Excel, now we need to make it look more appealing by changing all tile/wall types from numbers to actual words.

| Map Color In Hex: | ID: | Paint Color: |
|-------------------|-----|--------------|
|#976B4B|TILE: 0|None|
|#970000|TILE: 0|Red|
|#974B00|TILE: 0|Orange|
|#979700|TILE: 0|Yellow|
|#4B9700|TILE: 0|Lime|
|#009700|TILE: 0|Green|
|#00974B|TILE: 0|Teal|
|#009797|TILE: 0|Cyan|
|exc..|||

To do this, we can run our dumped data back through the game and sort out the tile and wall names. Using `ProcessIncomingMessage()` we can execute some code when a chat message is sent. It's a quick and non-problematic method.  To start this, we will need to read each line of a text file, searching for 

<details><summary>Show Code</summary>
 
```c#
public void ProcessIncomingMessage(ChatMessage message, int clientId)
{
    try
    {
        foreach (string line in File.ReadAllLines(@"C:\Program Files (x86)\Steam\steamapps\common\Terraria\colors.txt"))
        {
            // Get first four chars of string
            if (line.Substring(0, 4) == "WALL")
            {
                // Adjust strings
                string wallid = line.Replace("WALL: ", "");
                if (wallid.Length == 1)
                {
                    // Save text
                    File.AppendAllText(@"C:\Program Files (x86)\Steam\steamapps\common\Terraria\colorsOut.txt", string.Concat(new object[]{"WALL: ", wallid, "   (", Terraria.ID.WallID.Search.GetName(int.Parse(wallid)), ")", Environment.NewLine}));
                }
                else if (wallid.Length == 2)
                {
                    // Save text
                    File.AppendAllText(@"C:\Program Files (x86)\Steam\steamapps\common\Terraria\colorsOut.txt", string.Concat(new object[]{"WALL: ", wallid, "  (", Terraria.ID.WallID.Search.GetName(int.Parse(wallid)), ")", Environment.NewLine}));
                }
                else if (wallid.Length == 3)
                {
                    // Save text
                    File.AppendAllText(@"C:\Program Files (x86)\Steam\steamapps\common\Terraria\colorsOut.txt", string.Concat(new object[]{"WALL: ", wallid, " (", Terraria.ID.WallID.Search.GetName(int.Parse(wallid)), ")", Environment.NewLine}));
                }
            }
            else if (line.Substring(0, 4) == "TILE")
            {
                // Adjust strings
                string tileid = line.Replace("TILE: ", "");
                if (tileid.Length == 1)
                {
                    // Save text
                    File.AppendAllText(@"C:\Program Files (x86)\Steam\steamapps\common\Terraria\colorsOut.txt", string.Concat(new object[]{"TILE: ", tileid, "   (", Terraria.ID.TileID.Search.GetName(int.Parse(tileid)), ")", Environment.NewLine}));
                }
                else if (tileid.Length == 2)
                {
                    // Save text
                    File.AppendAllText(@"C:\Program Files (x86)\Steam\steamapps\common\Terraria\colorsOut.txt", string.Concat(new object[]{"TILE: ", tileid, "  (", Terraria.ID.TileID.Search.GetName(int.Parse(tileid)), ")", Environment.NewLine}));
                }
                else if (tileid.Length == 3)
                {
                    // Save text
                    File.AppendAllText(@"C:\Program Files (x86)\Steam\steamapps\common\Terraria\colorsOut.txt", string.Concat(new object[]{"TILE: ", tileid, " (", Terraria.ID.TileID.Search.GetName(int.Parse(tileid)), ")", Environment.NewLine}));
                }
            }
        }

        MessageBox.Show("Task Completed");
    }
    catch (Exception)
    {
        MessageBox.Show("Error Saving Data");
    }
}
```
</details>

After importing back into Excel, bellow will be the format of our new table.

| ID: | Paint Color: | Map Color In Hex: |
|-----|--------------|-------------------|
|TILE: 0 (Dirt)|None|976B4B|
|TILE: 0 (Dirt)|Red|970000|
|TILE: 0 (Dirt)|Orange|974B00|
|TILE: 0 (Dirt)|Yellow|979700|
|TILE: 0 (Dirt)|Lime|4B9700|
|TILE: 0 (Dirt)|Green|009700|
|TILE: 0 (Dirt)|Teal|00974B|
|TILE: 0 (Dirt)|Cyan|009797|
|exc..|||


 **Step5: Sorting Based On Color**
 
 The final step in creating a nice spreadsheet is to add some sorting. Sorting helps to keep everything organized and allows for many other collections options. As it stands right now, our this is only being sorted by Tile ID. A few nice methods would be to sort by hue, step sorting, and Inverted step sorting. Unfortunately, this starts to get very complicated in explaining, but to save time, I have gone ahead and created a program that takes this spreadsheet and automatically sorts them. If you wish to read into how these color sorting algorithms work, [Alan Zucconi](https://www.alanzucconi.com/ "View all posts by Alan Zucconi") wrote a nice acritical on it [here](https://www.alanzucconi.com/2015/09/30/colour-sorting/).
 
Using this sorting sorting tool, you can select to sort by HUE, HSV, and inverted HSV. Simply paste, press convert, and copy/paste back to excel. 
<p align="center">
  <img width="799" height="393" src="https://user-images.githubusercontent.com/33048298/180627810-4b43d413-0899-4974-ac9a-73f2d87ac01e.PNG">
</p>

Without sorting the colors, we will get a result close to the image bellow.
![sort_random](https://user-images.githubusercontent.com/33048298/180627851-49cd4897-0ec9-43bd-92a5-b6ca94dd1739.png)

Using some basic Hue over HSV, we get a more so `step sorting over a luminosity` style of sorting.
![Step](https://user-images.githubusercontent.com/33048298/180627893-5584ed6d-d0a4-4e53-a75f-c8adb3ad40ef.png)

Inverting this step function will grant us something closer to the image bellow.
![ReverseStep](https://user-images.githubusercontent.com/33048298/180627900-7595697e-05f2-42c5-8e24-26843c376427.png)


 **Step6: Coloring Cells In Excel**
 
 On this extra final step, I will be showing how to change each
 cell to its respected color. For any cell that contains a hex color code, we can simply use a script to programmatically change each cell color. Within the developer tab of excel, you can press `View code`. This will open an VBA code processor. Within this we want to make a way to loop through each populated cell that contains 6 numbers. Using this we attempt to change the interior color to the cells respected hex value.

<details><summary>Show Code</summary>
 
```vba
Private Sub Worksheet_Change(ByVal Target As Range)
    On Error GoTo bm_Safe_Exit
    Application.EnableEvents = False
    Dim rng As Range, clr As String
    For Each rng In Target
        If Len(rng.Value2) = 6 Then
            clr = rng.Value2
            rng.Interior.Color = _
              RGB(Application.Hex2Dec(Left(clr, 2)), _
                Application.Hex2Dec(Mid(clr, 3, 2)), _
                Application.Hex2Dec(Right(clr, 2)))
        End If
    Next rng
    
bm_Safe_Exit:
    Application.EnableEvents = True
End Sub
```
</details>

<p align="center">
  <img src="https://user-images.githubusercontent.com/33048298/180628858-5fcdac97-4c78-4022-a40e-ff3e13c72c00.PNG">
</p>
