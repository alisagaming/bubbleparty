#!/bin/bash

while getopts ":ac" opt; do
  case $opt in
    a)
	scp -i ~/.ssh/kk2.pem build.xml ubuntu@lqserver.brandomania.tv:lqserver/
	scp -i ~/.ssh/kk2.pem -r lib/* ubuntu@lqserver.brandomania.tv:lqserver/lib/
	scp -i ~/.ssh/kk2.pem -r config/* ubuntu@lqserver.brandomania.tv:lqserver/config/
      ;;
    c)
	scp -i ~/.ssh/kk2.pem -r config/* ubuntu@lqserver.brandomania.tv:lqserver/config/
      ;;
    \?)
      ;;
  esac
done

scp -i ~/.ssh/kk2.pem -r build/jar/* ubuntu@lqserver.brandomania.tv:lqserver/build/jar/
