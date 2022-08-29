using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DotAvifConverter;

/// <summary>
/// A executer for `cavif` which is a Encoder/converter for AVIF images.
/// Please have the native `cavif` libraries next to your application.
/// Grab prebuilt `cavif` from here: https://github.com/kornelski/cavif-rs/releases
/// </summary>
public static class AvifConverter
{
	private static bool RootBinaryChecked = false;
	private static string? BinPath = null;
	private static string BinCavifRoot = "." + Path.PathSeparator + "runtimes";
	private const string BinCavifWindows = "\\win\\cavif.exe";
	private const string BinCavifLinux = "/linux-generic/cavif";
	private const string BinCavifMac = "/mac/cavif";

	/// <summary>
	/// Sets root folder for `cavif` binary files. By default it is `runtimes`.
	/// </summary>
	public static void SetBinaryRoot(string root)
	{
		CheckBinaryRoot(root);
		BinCavifRoot = root;
	}

	/// <summary>
	/// Converts PNG/JPEG to AVIF format.
	/// </summary>
	public static Task<AvifConvertResult> EncodeImage(string imageFile, string? outputImageFile = null, AvifConverterOptions? options = null)
	{
		if (!RootBinaryChecked)
		{
			CheckBinaryRoot(BinCavifRoot);
			RootBinaryChecked = true;
		}

		var binary = GetConverterPath();
		var arguments = GenerateArgument(imageFile, outputImageFile, options);

		return RunEncodeProcessAsync(binary, arguments);
	}

	private static void CheckBinaryRoot(string root)
	{
		string path;
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			path = BinCavifWindows;
		}
		else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
		{
			path = BinCavifLinux;
		}
		else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
		{
			path = BinCavifMac;
		}
		else
		{
			throw new PlatformNotSupportedException();
		}

		string binPathCheck = root + path;
		if (!File.Exists(binPathCheck))
		{
			throw new FileNotFoundException($"cavif binary \"{path}\" not found in the path.");
		}
	}

	private static string GenerateArgument(string imageFile, string? outputImage = null, AvifConverterOptions? options = null)
	{
		var args = new List<string>(8) { };

		if (options != null)
		{
			if (!options.EmitMesage)
				args.Add("--quiet");

			if (options.Overwrite)
				args.Add("--overwrite");

			if (options.Speed.HasValue)
				args.Add("--speed " + options.Speed.Value);

			if (options.Quality.HasValue)
				args.Add("--quality " + options.Quality.Value);

			if (options.ColorRgb == true)
				args.Add("--color rgb");

			if (options.DirtyAlpha == true)
				args.Add("--dirty-alpha");
		}
		else
		{
			args.Add("--quiet");
			args.Add("--overwrite");
		}

		if (outputImage != null)
			args.Add($"-o \"{outputImage}\"");

		args.Add($"\"{imageFile}\"");

		return string.Join(" ", args);
	}

	private static string GetConverterPath()
	{
		if (BinPath != null)
			return BinPath;

		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			BinPath = BinCavifRoot + BinCavifWindows;
		}
		else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
		{
			BinPath = BinCavifRoot + BinCavifLinux;
		}
		else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
		{
			BinPath = BinCavifRoot + BinCavifMac;
		}
		else
		{
			throw new PlatformNotSupportedException();
		}
		return BinPath;
	}

	private static Task<AvifConvertResult> RunEncodeProcessAsync(string aviflibExe, string arguments)
	{
		var tcs = new TaskCompletionSource<AvifConvertResult>();

		try
		{
			var process = new Process
			{
				StartInfo =
				{
					FileName = aviflibExe,
					Arguments = arguments,
					WindowStyle = ProcessWindowStyle.Hidden,
					UseShellExecute = false,
					RedirectStandardOutput = true,
					CreateNoWindow = true
				},
				EnableRaisingEvents = true
			};

			process.Exited += (sender, args) =>
			{
				var line = "";
				while (!process.StandardOutput.EndOfStream)
					line += process.StandardOutput.ReadLine() + Environment.NewLine;

				var success = process.ExitCode == 0;

				tcs.SetResult(new AvifConvertResult(success, line));
				process.Dispose();
			};

			process.Start();
		}
		catch (Exception ex)
		{
			return Task.FromResult(new AvifConvertResult(false, ex.Message));
		}

		return tcs.Task;
	}

}
