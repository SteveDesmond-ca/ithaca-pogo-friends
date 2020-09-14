#!/usr/bin/env bash
set -e

dotnet run
git commit -am "Update friends.json"
git push