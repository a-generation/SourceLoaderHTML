# SourceLoaderHTML

A CLI tool written in C# for downloading all resources (scripts, styles, images) from a website, with the ability to ignore specified file types.

## Features

- Downloads all resource files from a specified website (e.g., images, CSS, JS).
- Allows filtering of files by extension (e.g., ignoring `.css`, `.js`, `.woff`).
- Saves downloaded resources in a specified folder.
- Logs progress during downloading (shows the number of files found and download status).

## Usage

```bash
sourceloaderhtml.exe "url" "output folder" "ignored extensions"
```

- **url**: The website URL to download resources from.
- **output folder**: The folder where all the downloaded files will be saved.
- **ignored extensions**: Space-separated list of file extensions to ignore during download.

### Example

```bash
sourceloaderhtml.exe "https://www.example.com/" "images" "css js woff"
```

In this example, the program will download all resources except `.css`, `.js`, and `.woff` files from the given URL and save them in the `images` folder.

### Download Progress

The tool logs progress in the terminal, showing how many files have been found and whether each file was successfully downloaded.

Example output:

```bash
Found 87 resources to download (after filtering).
(1/87) Downloading resource: https://example.com/script.js
(1/87) File script.js successfully downloaded.
(2/87) Downloading resource: https://example.com/style.css
Error downloading https://example.com/style.css: <error message>
...
Download complete.
```

## Installation

To build and run the program:

1. Clone or download the repository.
2. Open the project in your favorite C# IDE (e.g., Visual Studio).
3. Build the project.
4. Run the compiled executable with the desired arguments.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
