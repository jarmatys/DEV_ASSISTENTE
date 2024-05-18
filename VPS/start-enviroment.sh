#!/bin/bash

d=false
s=false
a=false
p=false

while [[ $# -gt 0 ]]; do
  case $1 in
    -d)
      d=true
      shift
      ;;
    -s)
      s=true
      shift
      ;;
    -a)
      a=true
      shift
      ;;
    -p)
      p=true
      shift
      ;;
    *)
      echo "Invalid option: $1"
      exit 1
      ;;
  esac
done

# Execute the corresponding docker-compose command
if $d; then
  docker compose -f ./docker-compose.yml --profile database up
elif $s; then
  docker compose -f ./docker-compose.yml --profile setup up -d
elif $a; then
  docker compose -f ./docker-compose.yml --profile app up -d
elif $p; then
  docker compose -f ./docker-compose.yml --profile playground up
else
  echo "Please provide a profile to start the environment: -s, -d, -p, -a (setup, database, playground, app)"
fi