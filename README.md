<div align="center">

# Tale <br>

### A story-writer for your terminal.
</div>

## Installation
Installing the program is very easy. Download your preffered native OS Tale:
[Tale-Windows](https://github.com/Itsmemonzu/Tale/releases/download/v0.3/tale-win.zip),
[Tale-Linux](https://github.com/Itsmemonzu/Tale/releases/download/v0.3/tale-linux.zip),
[Tale-OSX](https://github.com/Itsmemonzu/Tale/releases/download/v0.3/tale-osx.zip)

After downloading the .zip file, extract it and there you have it! Tale is now installed. 
Please note: If your OS is not listed above, you will have to build the program your self by downloading the source code.
<br>

## Building
This program is dependant on [Fluent Command Line Parser](https://github.com/fclp/fluent-command-line-parser). To build the program, you need to use:

```bash
$ dotnet publish -r win-x64 -c Release
```

If you want to make it self-contained then add `--self-contained true` at the end. Example:

```bash
$ dotnet publish -r win-x64 -c Release --self-contained true
```

Make sure to replace `win-x64` with your OS.

Finally, Your build will be located in: `Tale\bin\Release\net7.0\<yourOS>\publish`

<br>

## Running

Once the build is complete, you can simply run your terminal in the build directory and type:

```bash
# Dreate Journal (--date is optional)
$ tale --create newJournal --date 01/01/2023 

# View Journal
$ tale --view newJournal

# Edit Journal
$ tale --edit newJournal
    Doing this will ask you to enter some details.
    - Please write the part that you want to edit: (Example: Hello)
    - You want to replace it with: (Example: Hi)
    Please note that you need to enter a valid prompt that exists in the Journal. 

# Delete Journal
$ tale --delete newJournal

# Show List
$ tale --list

# Show commands
$ tale --cmd
```

<br>

## Contribution
Contributing to Tale is simple. You have to fork the repository and clone it. Make your changes. After you are done, just push the changes to your fork and make a pull request. 

We hope that you will be making some amazing changes!

<br>

## License

Licensed under the [MIT License](./LICENSE).