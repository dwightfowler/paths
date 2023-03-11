"""
Parse the PATH environment variable and show each path on a single line. \
If the path contains any environment variables, expand them before displaying.
"""
import os
import re
import argparse
import platform

if os.name == 'nt' and platform.release() == '10' and platform.version() >= '10.0.14393':
    # Fix ANSI color in Windows 10 version 10.0.14393 (Windows Anniversary Update)
    import ctypes
    kernel32 = ctypes.windll.kernel32
    kernel32.SetConsoleMode(kernel32.GetStdHandle(-11), 7)

def showpaths():
    """
    Show each path
    """
    path = os.getenv('PATH')
    paths = path.split(';')
    p_set = re.compile(r'%\w+%')
    parser = argparse.ArgumentParser(description='Show each entry in the \
    PATH environment variable on a separate line')
    parser.add_argument('-s', '--sort', action='store_true', help='sort the list of paths')
    for i, val in enumerate(paths):
        if not val:
            continue
        value = p_set.finditer(val)
        if not value:
            continue
        for match in value:
            var = match.group()
            old = var.replace('%', '')
            new = os.getenv(old)
            if not new:
                continue
            tmp = val.replace(var, new)
            paths[i] = tmp

    args = parser.parse_args()
    if args.sort:
        paths.sort(key=str.lower)
    # clear the screen
    print('\033[2J')
    for i in paths:
        if len(i) <= 0:
            continue
        if (os.path.exists(i)):
            print('\033[92m', end='')
        else:
            print('\033[31m', end='')
        print(i)
    print('\033[0m', end='')

showpaths()
