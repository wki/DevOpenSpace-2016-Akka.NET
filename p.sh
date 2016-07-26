#!/bin/bash
dir=`dirname $0`
[ -f $dir/.paket/paket.exe ] || mono $dir/.paket/paket.bootstrapper.exe
mono $dir/.paket/paket.exe $*
