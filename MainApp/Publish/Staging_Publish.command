#Setting current dir to this file's one
cd $(dirname $0)
#loading variables
source ./Settings.txt

echo 
echo "\033[1;33m"
echo "****************************************"
echo "****** PUBLISHING STAGING PROJECT ******"
echo "****************************************"
echo
echo "  -- Server address: ${SERVER_ADDRESS}"
echo "  -- SSH User: ${SERVER_SSH_USER}"
echo "  -- WEB APP URLS: ${STAGING_URLS}"
echo 
echo "****************************************"
echo "\033[0m"

#Cleaning local publish dir
rm -rf $PROJECT_PATH/Publish/Staging/*

#Setting up specific port for program.cs
sed -i .bak 's|//SCRIPT_PORT|webBuilder.UseUrls("'"$STAGING_URLS"'"); //BYSCRIPT|' $PROJECT_PATH/Program.cs

#Local Publishing
dotnet publish $PROJECT_PATH --output $PROJECT_PATH/Publish/Staging --configuration Release  --runtime linux-x64 --self-contained true -p:PublishSingleFile=true

EXIT_CODE=$?

#Reverting original port for program.cs
STAGING_URLS_ESC=$(sed 's/[\*\.]/\\&/g' <<<"$STAGING_URLS")
sed -i .bak 's|webBuilder.UseUrls("'"$STAGING_URLS_ESC"'"); //BYSCRIPT|//SCRIPT_PORT|' $PROJECT_PATH/Program.cs

#Exit if there were publishing errors
if [ $EXIT_CODE -gt 0 ]
then
	echo
	echo "\033[0;31m******  Local publishing failed with exit code - ${EXIT_CODE}  ******\033[0m"
	echo
	exit $EXIT_CODE
fi

echo
echo "\033[0;32m****** Local publishing successful ******\033[0m"
echo

#Checking if app dir exists on remote server
if ssh $SERVER_SSH_USER@$SERVER_ADDRESS '[ ! -d $SERVER_APPS_DIR/test ]'
then
   ssh $SERVER_SSH_USER@$SERVER_ADDRESS "mkdir $SERVER_APPS_DIR/test"
fi

#Cleaning remote server app dir	
ssh $SERVER_SSH_USER@$SERVER_ADDRESS "rm -rf ${SERVER_APPS_DIR}/test/*"
#Copying files to remote server app dir
scp -r $PROJECT_PATH/Publish/Staging/* $SERVER_SSH_USER@$SERVER_ADDRESS:$SERVER_APPS_DIR/test
	
echo
echo "\033[0;32m****** Publishing complete ******\033[0m"
echo

#Executing program if needed	
while true; do
	read -p "Do you wish to execute this program?[y/n]" yn
    case $yn in
    	[Yy]* ) ssh -t $SERVER_SSH_USER@$SERVER_ADDRESS "cd ${SERVER_APPS_DIR}/test; ./${APP_NAME}"; exit 0;;
      	[Nn]* ) exit 0;;
	   	 * ) echo "Please answer yes or no.";;
    esac
done