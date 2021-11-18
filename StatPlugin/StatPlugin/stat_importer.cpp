#include "stat_importer.h"

using namespace statimporter;
using namespace std;


void STAT_IMPORTER::read_stats(string fileName) {
	inFile.open(fileName);

	if (!inFile) {
		cerr << ("Unable to open file " + fileName);
		return;
	}

	string nextLine;
	while (getline(inFile, nextLine)) {
		// read in stats
		if (nextLine.find(',') != -1) {
			string delim = ",";
			vector<string> attacks = split(nextLine, delim);
			string attack_type = attacks[0];
			attacks.erase(attacks.begin());

			map<string,vector<int>> initmap;
			// attacks_list.insert(attack_type,initmap);
			attacks_list[attack_type] = initmap;
			for(string attack : attacks) {
				vector<string> values = split(attack,":");
				vector<int> dmgs;
				dmgs.push_back(stoi(values[1]));
				dmgs.push_back(stoi(values[2]));
				// attacks_list.get(attack_type).insert(values[0],dmgs);

				attacks_list[attack_type][values[0]] = dmgs;
			}
		}
		
		else {
			string delim = ":";
			string field = nextLine.substr(0, nextLine.find(delim));
			string value = nextLine.substr(nextLine.find(delim)+1, nextLine.length());

			stats[field] = stoi(value);
		}	
	}
	inFile.close();
}

// for string delimiter
// https://stackoverflow.com/questions/14265581/parse-split-a-string-in-c-using-string-delimiter-standard-c
vector<string> STAT_IMPORTER::split (string s, string delimiter) {
    size_t pos_start = 0, pos_end, delim_len = delimiter.length();
    string token;
    vector<string> res;

    while ((pos_end = s.find (delimiter, pos_start)) != string::npos) {
        token = s.substr(pos_start, pos_end - pos_start);
        pos_start = pos_end + delim_len;
        res.push_back(token);
    }

    res.push_back (s.substr (pos_start));
    return res;
}


void STAT_IMPORTER::create_player() {

}


int main(int argc, char *argv[]) {
	STAT_IMPORTER stat_imp;
	
	stat_imp.read_stats("../Squirtle.txt");
	//print stats
	for (auto it = stat_imp.stats.begin(); it != stat_imp.stats.end(); ++it)
		std::cout << it->first << " => " << it->second << '\n';

	//print attack list
	for (auto it = stat_imp.attacks_list.begin(); it != stat_imp.attacks_list.end(); ++it){
		std::cout << it->first << " => ";
		for (auto it2 = it->second.begin(); it2 != it->second.end(); ++it2){
			std::cout << it2->first << " , "; //<< it2->second << '\n';
			for(int i : it2->second) {
				std::cout << i;
			}
		}
	}
			
	return 0;
}