# Dot Avif Image Convertor
An `.avif` image convertor for .net. Uses [cavif](https://github.com/kornelski/cavif-rs) to do that.


## [NuGet Package](https://www.nuget.org/packages/DotAvifConverter)
```
PM> Install-Package DotAvifConverter
```

## Installation


1 - First you need to download the `cavif` prebuilt files and place them next to your application.
<br/>
[Download from here](https://github.com/kornelski/cavif-rs/releases)

2 - Make sure the `cavif` files are placed correctly e.g. like this:

```
./YourApplicationHere.exe
./YourApplicationHere.dll
./runtimes/win/cavif.exe
./runtimes/mac/cavif
./runtimes/linux-generic/cavif
```

You can change the root location of `runtimes` using `AvifConverter.SetBinaryRoot` method.

3 - You can call the `EncodeImage` method now.

## How to use

After installing the package, you only need to call the `AvifConverter.EncodeImage` method.

**Simple configurations:**
```csharp
await AvifConverter.EncodeImage("source.jpeg");
```
This will generate `source.avif` next to the source.

**Setting the output target**
```csharp
await AvifConverter.EncodeImage("source.jpeg", "output.avif");
```

**More options**

Using `AvifConverterOptions` you can customize the configuration.

```csharp
await AvifConverter.EncodeImage("source.jpeg", "output.avif", new AvifConverterOptions
{
	Speed = 6,
	Quality = 80
});
```

**Properties of `AvifConverterOptions`**
Some copies are from [cavif repo](https://github.com/kornelski/cavif-rs):

| Property | Description |
| -------- | -------- |
| Quality     | Quality from 1 (worst) to 100 (best), the default value is 80. The numbers have different meaning than JPEG's quality scale. Beware when comparing codecs. There is no lossless compression support.     |
| Speed     | Encoding speed between 1 (best, but slowest) and 10 (fastest, but a blurry mess), the default value is 4. Speeds 1 and 2 are unbelievably slow, but make files ~3-5% smaller. Speeds 7 and above degrade compression significantly, and are not recommended.     |
| Overwrite     | Replace files if there's .avif already. By default the existing files are overwritten.     |
| DirtyAlpha     | Preserve RGB values of fully transparent pixels (not recommended). By default irrelevant color of transparent pixels is cleared to avoid wasting space.     |
| ColorRgb     | Encode using RGB instead of YCbCr color space. Makes colors closer to lossless, but makes files larger. Use only if you need to avoid even smallest color shifts.     |
| EmitMesage     | Should generate output message     |
