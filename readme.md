# Instructions

Download the source code and compile it in VS.
Setup a folder with the drink images and save the path in the appSettings file in the setting "RawImagesFolder"
Setup another folder for the images cache and set the corresponding setting in appSettings

The provided appSettings will setup the rest Api with the uri http://localhost:8888/api/images

To use the API, the parameter "imageName" must be provided. This parameter is the name of the image file (with or without the png extension)

If the user wants to resize the image, he can pass the parameters horizontalSize and/or verticalSize. If both are passed and they don't have
the same horiz to vertical rate as the raw image, only one of the 2 parameters is used (the one that makes the image smaller)

If the user wants to add a watermark, the parameter "watermark" has to be provided

## Some example uses:

- original image with no resizing and no watermark
http://localhost:8888/api/images?imageName=01_04_2019_001111

- resized image with 400 pixels of width
http://localhost:8888/api/images?imageName=01_04_2019_001111&horizontalSize=400

- resized image with 400 pixels of height
http://localhost:8888/api/images?imageName=01_04_2019_001111&verticalSize=400

- resized image with 400 pixels of height and a watermark of "This is a watermark"
http://localhost:8888/api/images?imageName=01_04_2019_001111&verticalSize=400&watermark="This is a watermark"