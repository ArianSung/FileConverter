// SettingsManager.cs
using System.IO;

public static class SettingsManager
{
    private static readonly string _appName = "FileConverter";
    private static readonly string _fileName = "settings.ini";
    private static readonly string _settingsPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), _appName);
    private static readonly string _filePath = Path.Combine(_settingsPath, _fileName);

    public static async Task SaveSettingsAsync(HashSet<string> excludedExtensions)
    {
        try
        {
            // 설정 폴더가 없으면 생성
            Directory.CreateDirectory(_settingsPath);

            // HashSet을 "dll,exe,bin" 형태의 단일 문자열로 변환
            string extensionsLine = "ExcludedExtensions=" + string.Join(",", excludedExtensions);

            await File.WriteAllTextAsync(_filePath, extensionsLine);
        }
        catch (Exception)
        {
            // 오류가 발생해도 프로그램이 멈추지 않도록 처리 (예: 권한 문제)
            // 실제 배포 시에는 로그를 남기는 것이 좋습니다.
        }
    }

    public static async Task<HashSet<string>> LoadSettingsAsync()
    {
        var defaultExtensions = new HashSet<string>
    {
        "dll", "exe", "bin", "dat", "sys", "jpg", "jpeg", "png",
        "gif", "bmp", "mp3", "mp4", "avi", "zip", "rar"
    };

        if (!File.Exists(_filePath))
        {
            return defaultExtensions;
        }

        try
        {
            string line = await File.ReadAllTextAsync(_filePath);

            if (line.StartsWith("ExcludedExtensions="))
            {
                string values = line.Substring("ExcludedExtensions=".Length);

                // 만약 파일 내용은 있지만 값이 비어있다면, 무시하고 기본값을 사용하도록 개선
                if (string.IsNullOrEmpty(values))
                {
                    return defaultExtensions;
                }

                var loadedExtensions = new HashSet<string>(
                    values.Split(','),
                    StringComparer.OrdinalIgnoreCase);

                return loadedExtensions;
            }
        }
        catch (Exception)
        {
            // 파일이 손상되었거나 읽을 수 없는 경우
        }

        return defaultExtensions; // 로드 실패 시 기본값 반환
    }
}