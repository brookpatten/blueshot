param(
    [Parameter(Mandatory=$true)]
    [string]$RepositoryRootPath,
    [Parameter(Mandatory=$true)]
    [string]$BuildArtifactsPath,
    [Parameter(Mandatory=$true)]
    [string]$OutputPath
)

# Create portable directory
New-Item -ItemType Directory -Path "$OutputPath" -Force | Out-Null

# Copy checksum.SHA256
Copy-Item "$BuildArtifactsPath\checksum.SHA256" "$OutputPath" -Force
# Copy blueshot.exe
Copy-Item "$BuildArtifactsPath\blueshot.exe" "$OutputPath" -Force
# Copy blueshot.exe.config
Copy-Item "$BuildArtifactsPath\blueshot.exe.config" "$OutputPath" -Force

# Copy all dlls
Copy-Item "$BuildArtifactsPath\*.dll" "$OutputPath" -Force

# Copy emoji resources
Copy-Item "$BuildArtifactsPath\emojis.xml" "$OutputPath" -Force
Copy-Item "$BuildArtifactsPath\Twemoji.Mozilla.ttf" "$OutputPath" -Force

# Copy help files
New-Item -ItemType Directory -Path "$OutputPath\Help" -Force | Out-Null
Copy-Item "$RepositoryRootPath\src\Blueshot\Languages\*.html" "$OutputPath\Help" -Force

# Copy languages files
New-Item -ItemType Directory -Path "$OutputPath\Languages" -Force | Out-Null
Copy-Item "$RepositoryRootPath\src\Blueshot\Languages\*.xml" "$OutputPath\Languages" -Force

# Create Dummy-INI
";dummy config, used to make blueshot store the configuration in this directory" | Set-Content "$OutputPath\blueshot.ini" -Encoding UTF8

# Create Dummy-defaults-INI
";In this file you should add your default settings" | Set-Content "$OutputPath\blueshot-defaults.ini" -Encoding UTF8

# Create Dummy-fixed-INI
";In this file you should add your fixed settings" | Set-Content "$OutputPath\blueshot-fixed.ini" -Encoding UTF8

# Copy license file
Copy-Item "$RepositoryRootPath\src\Blueshot-Installer\additional_files\license.txt" "$OutputPath" -Force

# Copy readme file
Copy-Item "$RepositoryRootPath\src\Blueshot-Installer\additional_files\readme.txt" "$OutputPath" -Force

# Copy and rename log config file
Copy-Item "$RepositoryRootPath\src\Blueshot\log4net-zip.xml" "$OutputPath\log4net.xml" -Force

# Copy Box Plugin
# New-Item -ItemType Directory -Path "$OutputPath\Languages\blueshot.Plugin.Box" -Force | Out-Null
# New-Item -ItemType Directory -Path "$OutputPath\Plugins\blueshot.Plugin.Box" -Force | Out-Null
# Copy-Item "$RepositoryRootPath\src\Blueshot.Plugin.Box\Languages\language_box*.xml" "$OutputPath\Languages\blueshot.Plugin.Box" -Force
# Copy-Item "$BuildArtifactsPath\Plugins\Blueshot.Plugin.Box\Blueshot.Plugin.Box.dll" "$OutputPath\Plugins\Blueshot.Plugin.Box" -Force

# Copy Confluence Plugin
New-Item -ItemType Directory -Path "$OutputPath\Languages\blueshot.Plugin.Confluence" -Force | Out-Null
New-Item -ItemType Directory -Path "$OutputPath\Plugins\blueshot.Plugin.Confluence" -Force | Out-Null
Copy-Item "$RepositoryRootPath\src\Blueshot.Plugin.Confluence\Languages\language_confluence*.xml" "$OutputPath\Languages\blueshot.Plugin.Confluence" -Force
Copy-Item "$BuildArtifactsPath\Plugins\blueshot.Plugin.Confluence\blueshot.Plugin.Confluence.dll" "$OutputPath\Plugins\blueshot.Plugin.Confluence" -Force

# Copy Dropbox Plugin
# New-Item -ItemType Directory -Path "$OutputPath\Languages\blueshot.Plugin.Dropbox" -Force | Out-Null
# New-Item -ItemType Directory -Path "$OutputPath\Plugins\blueshot.Plugin.Dropbox" -Force | Out-Null
# Copy-Item "$RepositoryRootPath\src\Blueshot.Plugin.Dropbox\Languages\language_dropbox*.xml" "$OutputPath\Languages\blueshot.Plugin.Dropbox" -Force
# Copy-Item "$BuildArtifactsPath\Plugins\blueshot.Plugin.Dropbox\blueshot.Plugin.Dropbox.dll" "$OutputPath\Plugins\blueshot.Plugin.Dropbox" -Force

# Copy ExternalCommand Plugin
New-Item -ItemType Directory -Path "$OutputPath\Languages\blueshot.Plugin.ExternalCommand" -Force | Out-Null
New-Item -ItemType Directory -Path "$OutputPath\Plugins\blueshot.Plugin.ExternalCommand" -Force | Out-Null
Copy-Item "$RepositoryRootPath\src\Blueshot.Plugin.ExternalCommand\Languages\language_externalcommand*.xml" "$OutputPath\Languages\blueshot.Plugin.ExternalCommand" -Force
Copy-Item "$BuildArtifactsPath\Plugins\blueshot.Plugin.ExternalCommand\blueshot.Plugin.ExternalCommand.dll" "$OutputPath\Plugins\blueshot.Plugin.ExternalCommand" -Force


# Copy Imgur Plugin
# New-Item -ItemType Directory -Path "$OutputPath\Languages\blueshot.Plugin.Imgur" -Force | Out-Null
# New-Item -ItemType Directory -Path "$OutputPath\Plugins\blueshot.Plugin.Imgur" -Force | Out-Null
# Copy-Item "$RepositoryRootPath\src\Blueshot.Plugin.Imgur\Languages\language_imgur*.xml" "$OutputPath\Languages\blueshot.Plugin.Imgur" -Force
# Copy-Item "$BuildArtifactsPath\Plugins\blueshot.Plugin.Imgur\blueshot.Plugin.Imgur.dll" "$OutputPath\Plugins\blueshot.Plugin.Imgur" -Force

# Copy Jira Plugin
New-Item -ItemType Directory -Path "$OutputPath\Languages\blueshot.Plugin.Jira" -Force | Out-Null
New-Item -ItemType Directory -Path "$OutputPath\Plugins\blueshot.Plugin.Jira" -Force | Out-Null
Copy-Item "$RepositoryRootPath\src\Blueshot.Plugin.Jira\Languages\language_jira*.xml" "$OutputPath\Languages\blueshot.Plugin.Jira" -Force
Copy-Item "$BuildArtifactsPath\Plugins\blueshot.Plugin.Jira\blueshot.Plugin.Jira.dll" "$OutputPath\Plugins\blueshot.Plugin.Jira" -Force
Copy-Item "$BuildArtifactsPath\Plugins\blueshot.Plugin.Jira\Dapplo.Jira.dll" "$OutputPath\Plugins\blueshot.Plugin.Jira" -Force
Copy-Item "$BuildArtifactsPath\Plugins\blueshot.Plugin.Jira\Dapplo.Jira.SvgWinForms.dll" "$OutputPath\Plugins\blueshot.Plugin.Jira" -Force

# Copy Office Plugin
New-Item -ItemType Directory -Path "$OutputPath\Plugins\blueshot.Plugin.Office" -Force | Out-Null
Copy-Item "$BuildArtifactsPath\Plugins\blueshot.Plugin.Office\blueshot.Plugin.Office.dll" "$OutputPath\Plugins\blueshot.Plugin.Office" -Force
