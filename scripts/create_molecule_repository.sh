SCRIPT_DIR=$(cd $(dirname $0); pwd)
UNITY_CLI="Unity -batchmode -projectPath $SCRIPT_DIR/.. -quit -logFile - ->&1"

function check_unity_exec(){
    type Unity > /dev/null || (echo "[ERROR] Unity executable can't be found in PATH. Add Unity directory into PATH" && exit 1)
}

function usage(){
    echo $1
    cat << EOT
Usage:
    $(basename $0) FILE_NAME

Description:
    Create new molecule repository file (.asset) in Assets/Galahad/Editor/Outputs

Arguments:
    FILE_NAME file name (e.g. repo.asset)

EOT
    exit 1
}

check_unity_exec
if [ "$1" == "" ]; then
    echo "[ERROR] Option argument is undefined."
    usage 
fi

if [ ! -d $SCRIPT_DIR/Temp ]; then mkdir $SCRIPT_DIR/Temp; fi
$UNITY_CLI -executeMethod Galahad.Editor.Cli.CreateMoleculeRepository $1
