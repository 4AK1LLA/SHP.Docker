Before using scripts you might need to open PowerShell as admin and execute command 'Set-ExecutionPolicy RemoteSigned'*. (If scripts ain't working)

* - On Windows 10, PowerShell includes four execution policies:
Restricted — Stops any script from running.
RemoteSigned — Allows scripts created on the device, but scripts created on another computer won't run unless they include a trusted publisher's signature.
AllSigned — All the scripts will run, but only if a trusted publisher has signed them.
Unrestricted — Runs any script without any restrictions.