#!/bin/bash
set -e
FROM="$(sh ./list.migrations.sh | tail -2 | head -1)"
./g.script.sh $FROM
