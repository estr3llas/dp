# dp (depex parser)

```
Parse/patch UEFI module's dependency expressions

Usage: dp [options] <filename>

Arguments:
  filename  The depex file to be processed.

Options:
  -h        Show help information.
  -d        Disassemble the given depex
  -o        File to receive the output
```

## Example of parsed depex:

```
[+] Disassembled output:

[i] Header:
---------------------------------
3A 00 00 1B

[i] Body:
---------------------------------
PUSH  EfiPeiMemoryDiscoveredPpiGuid            (f894643d-c449-42d1-8ea8-85bdd8c65bde)
PUSH  EfiPeiPcdPpiGuid                         (01f34d25-4de2-23ad-3ff3-36353ff323f1)
PUSH  1a266768-fd43-4e18-a88a-35c794c3910e
AND
AND
END
```
