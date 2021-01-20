#Setting current dir to this file's one
cd $(dirname $0)
#loading variables
source ./Settings.txt

echo 
echo "\033[1;33m"
echo "*******************************************"
echo "****** PUBLISHING PRODUCTION PROJECT ******"
echo "*******************************************"
echo
echo "  -- Server address: ${SERVER_ADDRESS}"
echo "  -- SSH User: ${SERVER_SSH_USER}"
echo "  -- WEB APP URLS: ${PRODUCTION_URLS}"
echo 
echo "****************************************"
echo "\033[0m"

#Cleaning local publish dir
rm -rf $PROJECT_PATH/Publish/Production/*

#Setting up specific port for program.cs
sed -i .bak 's|//SCRIPT_PORT|webBuilder.UseUrls("'"$PRODUCTION_URLS"'"); //BYSCRIPT|' $PROJECT_PATH/Program.cs

#Local Publishing
dotnet publish $PROJECT_PATH --output $PROJECT_PATH/Publish/Production --configuration Release  --runtime linux-x64 --self-contained true -p:PublishSingleFile=true

EXIT_CODE=$?

#Reverting original port for program.cs
PRODUCTION_URLS_ESC=$(sed 's/[\*\.]/\\&/g' <<<"$PRODUCTION_URLS")
sed -i .bak 's|webBuilder.UseUrls("'"$PRODUCTION_URLS_ESC"'"); //BYSCRIPT|//SCRIPT_PORT|' $PROJECT_PATH/Program.cs

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

ssh $SERVER_SSH_USER@$SERVER_ADDRESS "systemctl stop $APP_NAME.service"

#Checking if app dir exists on remote server
if ssh $SERVER_SSH_USER@$SERVER_ADDRESS "[ ! -d $SERVER_APPS_DIR/$APP_NAME ]"
then
   ssh $SERVER_SSH_USER@$SERVER_ADDRESS "mkdir $SERVER_APPS_DIR/$APP_NAME"
fi

#Cleaning remote server app dir	
ssh $SERVER_SSH_USER@$SERVER_ADDRESS "rm -rf ${SERVER_APPS_DIR}/$APP_NAME/*"
#Copying files to remote server app dir
scp -r $PROJECT_PATH/Publish/Production/* $SERVER_SSH_USER@$SERVER_ADDRESS:$SERVER_APPS_DIR/$APP_NAME
	
echo
echo "\033[0;32m****** Publishing complete ******\033[0m"
echo
echo "\033[0;32m****** Setting up systemd service ******\033[0m"

echo "[Unit]
Description=$SERVICE_DESCRIPTION

[Service]
# systemd will run this executable to start the service
ExecStart=$SERVER_APPS_DIR/$APP_NAME/$APP_NAME
WorkingDirectory=$SERVER_APPS_DIR/$APP_NAME
# to query logs using journalctl, set a logical name here
SyslogIdentifier=$APP_NAME

# Use your username to keep things simple.
# If you pick a different user, make sure dotnet and all permissions are set correctly to run the app
# To update permissions, use 'chown yourusername -R /srv/HelloWorld' to take ownership of the folder and files,
#       Use 'chmod +x /srv/HelloWorld/HelloWorld' to allow execution of the executable file
User=root

# ensure the service restarts after crashing
Restart=always
# amount of time to wait before restarting the service                        
RestartSec=1                            

# This environment variable is necessary when dotnet isn't loaded for the specified user.
# To figure out this value, run 'env | grep DOTNET_ROOT' when dotnet has been loaded into your shell.
Environment=DOTNET_ROOT=/opt/rh/rh-dotnet31/root/usr/lib64/dotnet  

[Install]
WantedBy=multi-user.target" | ssh $SERVER_SSH_USER@$SERVER_ADDRESS "cat > /etc/systemd/system/$APP_NAME.service"

ssh $SERVER_SSH_USER@$SERVER_ADDRESS "systemctl daemon-reload; systemctl start $APP_NAME.service; systemctl enable $APP_NAME.service"

echo "\033[0;32m"
echo "********************************************"
echo "******  Application is ready to use!  ******"
echo "********************************************"
echo "\033[0m"