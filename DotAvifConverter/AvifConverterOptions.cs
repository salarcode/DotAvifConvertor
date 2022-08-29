namespace DotAvifConverter;

/// <summary>
/// Read more about options here:
/// https://github.com/kornelski/cavif-rs
/// </summary>
public class AvifConverterOptions
{
	/// <summary>
	/// Quality from 1 (worst) to 100 (best), the default value is 80. The numbers have different meaning than JPEG's quality scale. Beware when comparing codecs. There is no lossless compression support.
	/// </summary>
	public int? Quality { get; set; }

	/// <summary>
	/// Encoding speed between 1 (best, but slowest) and 10 (fastest, but a blurry mess), the default value is 4. Speeds 1 and 2 are unbelievably slow, but make files ~3-5% smaller. Speeds 7 and above degrade compression significantly, and are not recommended.
	/// </summary>
	public int? Speed { get; set; }

	/// <summary>
	/// Replace files if there's .avif already. By default the existing files are overwritten.
	/// </summary>
	public bool Overwrite { get; set; } = true;

	/// <summary>
	/// Preserve RGB values of fully transparent pixels (not recommended). By default irrelevant color of transparent pixels is cleared to avoid wasting space.
	/// </summary>
	public bool? DirtyAlpha { get; set; }

	/// <summary>
	/// Encode using RGB instead of YCbCr color space. Makes colors closer to lossless, but makes files larger. Use only if you need to avoid even smallest color shifts.
	/// </summary>
	public bool? ColorRgb { get; set; }

	/// <summary>
	/// Generate output message
	/// </summary>
	public bool EmitMesage { get; set; }
}
