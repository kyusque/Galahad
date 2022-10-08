SCRIPT_DIR=$(cd $(dirname $0); pwd)
UNITY_CLI="Unity -batchmode -projectPath $SCRIPT_DIR/.. -quit -logFile $SCRIPT_DIR/Temp/log.txt"

function check_unity_exec(){
    type Unity > /dev/null || (echo "[ERROR] Unity executable can't be found in PATH. Add Unity directory into PATH" && exit 1)
}

function usage(){
    echo $1
    cat << EOT
Usage:
    $(basename $0) MOLECULE_REPOSITORY SMILES

Description:
    Create new molecule repository file (.asset) in Assets/Galahad/Editor/Outputs

Arguments:
    MOLECULE_REPOSITORY: asset file path in Assets folder (e.g. Assets/Galahad/Data/sample.asset)
    SMILES: SMILES string

EOT
    exit 1
}

check_unity_exec
if [ "$1" == "" ]; then
    echo "[ERROR] MOLECULE_REPOSITORY is undefined."
    usage 
fi
if [ "$2" == "" ]; then
    echo "[ERROR] SMILES is undefined."
    usage 
fi

if [ ! -d $SCRIPT_DIR/Temp ]; then mkdir $SCRIPT_DIR/Temp; fi
$UNITY_CLI -executeMethod Galahad.Editor.Cli.AddSmilesIntoMoleculeRepository $1 $2
