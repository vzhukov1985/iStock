cd $(dirname $0)
source ./Settings.txt

ssh -t $SERVER_SSH_USER@$SERVER_ADDRESS "cd ${SERVER_APPS_DIR}/$APP_NAME; ./${APP_NAME}"
exit 0;