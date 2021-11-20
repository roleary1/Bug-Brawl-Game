#include "stat_importer.h"

using namespace std;

extern "C" {
	void read_stats(list<const char*> statKeys, list<int> statValues, list<const char*> attackKeys,
		list<int> attackDmg, list<int> attackAccuracy, const char* fileName) {
		ifstream inFile;

		// map<string, int> stats;
        // map<std::string, map<std::string, vector<int>>> attacks_list;
		
		inFile.open(std::string(fileName));

		if (!inFile) {
			cerr << ("Unable to open file " + std::string(fileName));
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
				
				for (string attack : attacks) {
					vector<string> values = split(attack, ":");
					// attackKeys are the names of the attacks
					attackKeys.push_back(const_cast<char*>(values[0].c_str()));
					attackDmg.push_back(stoi(values[1]));
					attackAccuracy.push_back(stoi(values[2])); // NOTE: first 3 items are basic attacks, next 3 are special
				}
			}

			else {
				string delim = ":";
				string field = nextLine.substr(0, nextLine.find(delim));
				string value = nextLine.substr(nextLine.find(delim) + 1, nextLine.length());

				statKeys.push_back(const_cast<char*>(field.c_str()));
				statValues.push_back(stoi(value));
			}
		}
		inFile.close();
	}

	// for string delimiter
	// https://stackoverflow.com/questions/14265581/parse-split-a-string-in-c-using-string-delimiter-standard-c
	vector<string> split(string s, string delimiter) {
		size_t pos_start = 0, pos_end, delim_len = delimiter.length();
		string token;
		vector<string> res;

		while ((pos_end = s.find(delimiter, pos_start)) != string::npos) {
			token = s.substr(pos_start, pos_end - pos_start);
			pos_start = pos_end + delim_len;
			res.push_back(token);
		}

		res.push_back(s.substr(pos_start));
		return res;
	}
}


int main(int argc, char *argv[]) {
	// STAT_IMPORTER stat_imp;
	
	// stat_imp.read_stats("Squirtle.txt");
	// //print stats
	// for (auto it = stat_imp.stats.begin(); it != stat_imp.stats.end(); ++it)
	// 	std::cout << it->first << " => " << it->second << '\n';

	// //print attack list
	// for (auto it = stat_imp.attacks_list.begin(); it != stat_imp.attacks_list.end(); ++it){
	// 	std::cout << it->first << " => ";
	// 	for (auto it2 = it->second.begin(); it2 != it->second.end(); ++it2){
	// 		std::cout << it2->first << " , "; //<< it2->second << '\n';
	// 		for(int i : it2->second) {
	// 			std::cout << i;
	// 		}
	// 	}
	// }
			
	return 0;
}