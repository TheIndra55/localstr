# localstr

Tool to import and export from Tomb Raider games `locals.bin` files, tested with Tomb Raider: Legend and Anniversary.

## Usage

Drag your locals.bin file on the executable, it will export a .json file next to your file.

Edit the .json file and drag it on the executable again to convert it back to locals.bin.

```json
{
    "language": 0,
    "strings": [
        "",
        "Tomb Raider: Anniversary - Setup Dialog",
        "Select Rendering Device",
        "Select Resolution and colordepth",
        "Triple buffering",
        [...]
    ]
}
```